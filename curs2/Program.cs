using System;
using System.Linq;

namespace BvpSolver
{
    /// <summary>
    /// Класс решения краевой задачи второго порядка методом стрельбы
    /// </summary>
    internal static class Program
    {
        private const double Epsilon = 1e-12;
        private const double InitialStep = 0.1;
        private const double A = 0.0;
        private const double B = 1.0;
        private const double Ya = 1.0;
        private static readonly double Yb = Math.Exp(1);

        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        private static void Main()
        {
            Console.Title = "Решение краевой задачи методом стрельбы";
            DrawHeader("===== Краевая задача =====");
            Console.WriteLine("Уравнение: y'' = (exp(x) + y + y') / 3");
            Console.WriteLine($"Аналитическое решение: y(x) = exp(x), y'(x) = exp(x)");
            Console.WriteLine($"Граничные условия: y({A}) = {Ya}, y({B}) = {Yb}");
            Console.WriteLine($"y'({A}) (аналитическое) = {Math.Exp(A)}");
            Console.WriteLine($"y'({B}) (аналитическое) = {Math.Exp(B)}");
            Console.WriteLine($"Требуемая точность для y(B): |y(B) - Yb| < {Epsilon:E}\n");

            bool useRk4 = AskMethod();
            Console.WriteLine();

            var methodName = useRk4 ? "Рунге-Кутта 4-го порядка" : "Рунге-Кутта 2-го порядка";

            double integrationStepForKSearch;
            if (useRk4)
            {
                integrationStepForKSearch = 1e-4;
            }
            else
            {
                integrationStepForKSearch = 1e-6;
            }

            DrawHeader($"=== Метод стрельбы ({methodName}) для поиска y'(0) ===");
            Console.WriteLine($"Используемый шаг интегрирования в ShotFunction при поиске y'(0): {integrationStepForKSearch:E}");
            double k0 = FindInitialSlope(-10.0, 10.0, Epsilon, integrationStepForKSearch, useRk4);
            Console.WriteLine($"Найденное y'(0) (k0) = {k0:F14}\n");

            DrawHeader($"=== Подбор оптимального шага ({methodName}) для найденного y'(0) ===");
            double optimalH = AdjustStep(Epsilon, InitialStep, Ya, k0, useRk4);
            Console.WriteLine($"Оптимальный шаг: {optimalH:E}\n");

            DrawHeader($"=== Итоговый прогон ({methodName}) ===");
            var result = useRk4
                ? RungeKutta4(A, B, optimalH, Ya, k0, true)
                : RungeKutta2(A, B, optimalH, Ya, k0, true);

            Console.WriteLine($"\nРезультат в точке {B}: y = {result.Y:F14}, y' = {result.DY:F14}");
            Console.WriteLine($"Ожидаемое значение y({B}): {Yb:F14}");
            Console.WriteLine($"Разница |y(B) - Yb|: {Math.Abs(result.Y - Yb):E}");

            double expectedDYb = Math.Exp(B);
            Console.WriteLine($"Ожидаемое значение y'({B}): {expectedDYb:F14}");
            Console.WriteLine($"Разница |y'(B) - exp(B)|: {Math.Abs(result.DY - expectedDYb):E}\n");


            Console.ResetColor();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        private static void DrawHeader(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static bool AskMethod()
        {
            Console.WriteLine("Выберите метод интегрирования:");
            Console.WriteLine("1 - Рунге-Кутта 4-го порядка");
            Console.WriteLine("2 - Рунге-Кутта 2-го порядка");
            while (true)
            {
                Console.Write("Ваш выбор [1/2]: ");
                var key = Console.ReadKey(intercept: true).KeyChar;
                Console.WriteLine(key);
                if (key == '1') return true;
                if (key == '2') return false;
                Console.WriteLine("Неверный ввод, повторите.");
            }
        }

        private static (double F0, double F1) F(double x, double y, double dy)
        {
            return (dy, (Math.Exp(x) + y + dy) / 3.0);
        }

        private static (double Y, double DY) RungeKutta4(double a, double b, double h, double y0, double dy0, bool print)
        {
            if (h <= 0) throw new ArgumentOutOfRangeException(nameof(h), "Шаг h должен быть положительным.");

            int n = (int)Math.Ceiling(Math.Abs(b - a) / h);
            if (n == 0) n = 1;

            double actualH = (b - a) / n;

            double[] X = new double[n + 1];
            for (int i = 0; i <= n; ++i) X[i] = a + i * actualH;

            double y = y0, dy = dy0;

            if (print && X.Length > 0)
                Console.WriteLine($"x = {X[0]:F14} | y = {y:F14} | y' = {dy:F14}");

            for (int i = 1; i <= n; i++)
            {
                var (k10, k11) = F(X[i - 1], y, dy);
                var (k20, k21) = F(X[i - 1] + actualH / 2, y + actualH / 2 * k10, dy + actualH / 2 * k11);
                var (k30, k31) = F(X[i - 1] + actualH / 2, y + actualH / 2 * k20, dy + actualH / 2 * k21);
                var (k40, k41) = F(X[i - 1] + actualH, y + actualH * k30, dy + actualH * k31);

                y += actualH / 6 * (k10 + 2 * k20 + 2 * k30 + k40);
                dy += actualH / 6 * (k11 + 2 * k21 + 2 * k31 + k41);

                if (print)
                    Console.WriteLine($"x = {X[i]:F14} | y = {y:F14} | y' = {dy:F14}");
            }
            return (y, dy);
        }

        private static (double Y, double DY) RungeKutta2(double a, double b, double h, double y0, double dy0, bool print)
        {
            if (h <= 0) throw new ArgumentOutOfRangeException(nameof(h), "Шаг h должен быть положительным.");

            int n = (int)Math.Ceiling(Math.Abs(b - a) / h);
            if (n == 0) n = 1;
            double actualH = (b - a) / n;

            double[] X = new double[n + 1];
            for (int i = 0; i <= n; ++i) X[i] = a + i * actualH;

            double y = y0, dy = dy0;

            if (print && X.Length > 0)
                Console.WriteLine($"x = {X[0]:F14} | y = {y:F14} | y' = {dy:F14}");

            for (int i = 1; i <= n; i++)
            {
                var (k10, k11) = F(X[i - 1], y, dy);
                var (k20, k21) = F(X[i - 1] + actualH / 2, y + actualH / 2 * k10, dy + actualH / 2 * k11);

                y += actualH * k20;
                dy += actualH * k21;

                if (print)
                    Console.WriteLine($"x = {X[i]:F14} | y = {y:F14} | y' = {dy:F14}");
            }
            return (y, dy);
        }

        /// <summary>
        /// Осуществляет метод стрельбы для поиска начального dy по краевому условию.
        /// </summary>
        /// <param name="left">Левая граница интервала для k = y'(a).</param>
        /// <param name="right">Правая граница интервала для k = y'(a).</param>
        /// <param name="kIntervalTolerance">Требуемая точность для нахождения k.</param>
        /// <param name="integrationStep">Шаг интегрирования, используемый в ShotFunction.</param>
        /// <param name="useRk4">Использовать ли РК4 (true) или РК2 (false).</param>
        /// <returns>Найденное значение y'(a).</returns>
        private static double FindInitialSlope(double left, double right, double kIntervalTolerance, double integrationStep, bool useRk4)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Начальный интервал для k: [{left:F14}, {right:F14}], шаг интегрирования для ShotFunction = {integrationStep:E}, точность для k = {kIntervalTolerance:E}");
            Console.ResetColor();

            double fa = ShotFunction(left, integrationStep, useRk4);
            double fb = ShotFunction(right, integrationStep, useRk4);

            if (Math.Abs(fa) < Epsilon)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nКорень найден на левой границе: k = {left:F14}");
                Console.ResetColor();
                return left;
            }
            if (Math.Abs(fb) < Epsilon)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nКорень найден на правой границе: k = {right:F14}");
                Console.ResetColor();
                return right;
            }

            if (fa * fb >= 0)
            {
                Console.WriteLine($"Предупреждение: f(a) и f(b) одного знака. fa={fa:E}, fb={fb:E}. Поиск может быть некорректным.");
            }


            double mid = left;
            int iter = 0;
            const int maxIter = 200;

            while (Math.Abs(right - left) > kIntervalTolerance && iter < maxIter)
            {
                iter++;
                mid = (left + right) / 2;
                double fm = ShotFunction(mid, integrationStep, useRk4);

                if (Math.Abs(fm) < Epsilon)
                {
                    Console.WriteLine($"Достигнута целевая точность для y(B) с k = {mid:F14}. Отклонение = {fm:E}");
                    break;
                }

                if (fa * fm < 0)
                {
                    right = mid;
                    fb = fm;
                }
                else
                {
                    left = mid;
                    fa = fm;
                }
            }
            if (iter == maxIter)
            {
                Console.WriteLine("Предупреждение: Достигнуто максимальное количество итераций в FindInitialSlope.");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nЗавершение поиска k: итераций={iter}, интервал k = [{left:F14}, {right:F14}]");
            Console.ResetColor();
            return (left + right) / 2;
        }

        /// <summary>
        /// Вычисляет отклонение в точке b для заданного k и шага h (integrationStep)
        /// </summary>
        private static double ShotFunction(double k, double integrationStep, bool useRk4)
        {
            var result = useRk4
                ? RungeKutta4(A, B, integrationStep, Ya, k, false)
                : RungeKutta2(A, B, integrationStep, Ya, k, false);
            double error = result.Y - Yb;
            return error;
        }

        private static double AdjustStep(double eps, double initialH, double y0, double dy0, bool useRk4)
        {
            const double MinH = 1e-15;
            double h = initialH;

            Console.WriteLine($"Подбор шага для y'(0)={dy0:F14}, целевая ошибка для y(B) < {eps:E}");
            int iter = 0;
            const int maxIter = 100;

            while (h > MinH && iter < maxIter)
            {
                iter++;
                var result = useRk4
                    ? RungeKutta4(A, B, h, y0, dy0, false)
                    : RungeKutta2(A, B, h, y0, dy0, false);
                double error = Math.Abs(result.Y - Yb);
                Console.WriteLine($"Итерация {iter}: h = {h:E}, y(b) = {result.Y:F14}, ошибка |y(b)-Yb| = {error:E}");

                if (error < eps)
                {
                    Console.WriteLine($"Достигнута точность {eps:E}: оптимальный шаг = {h:E}");
                    return h;
                }
                h /= 2;
            }
            if (iter == maxIter && h > MinH)
            {
                Console.WriteLine("Предупреждение: Достигнуто максимальное количество итераций в AdjustStep, точность может быть не достигнута.");
            }
            else if (h <= MinH)
            {
                Console.WriteLine("Минимальный шаг достигнут, но требуемая точность не достигнута.");
            }
            return h;
        }
    }
}