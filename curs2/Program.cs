using System;
using System.Linq;

namespace BvpSolver
{
    /// <summary>
    /// Класс решения краевой задачи второго порядка методом стрельбы
    /// </summary>
    internal static class Program
    {
        private const double Epsilon = 1e-8;
        private const double Step = 0.1;
        private const double A = 0.0;
        private const double B = 1.0;
        private const double Ya = 1.0;
        private const double Yb = 2.718281828459045;

        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        private static void Main()
        {
            Console.Title = "Решение краевой задачи методом стрельбы";
            DrawHeader("===== Краевая задача =====");
            Console.WriteLine("Уравнение: y'' = (exp(x) + y + y') / 3");
            Console.WriteLine($"Граничные условия: y({A}) = {Ya}, y({B}) = {Yb}");
            Console.WriteLine($"Требуемая точность: {Epsilon}\n");

            bool useRk4 = AskMethod();
            Console.WriteLine();

            var methodName = useRk4 ? "Рунге-Кутта 4-го порядка" : "Рунге-Кутта 2-го порядка";
            DrawHeader($"=== Метод стрельбы ({methodName}) ===");
            double k0 = FindInitialSlope(-100.0, 100.0, useRk4);
            Console.WriteLine($"Найденное y'(0) = {k0:F13}\n");

            DrawHeader($"=== Подбор оптимального шага ({methodName}) ===");
            double optimalH = AdjustStep(Epsilon, Step, Ya, k0, useRk4);
            Console.WriteLine($"Оптимальный шаг: {optimalH:E}\n");

            DrawHeader($"=== Итоговый прогон ({methodName}) ===");
            var result = useRk4
                ? RungeKutta4(A, B, optimalH, Ya, k0, true)
                : RungeKutta2(A, B, optimalH, Ya, k0, true);

            Console.WriteLine($"Результат в точке {B}: y = {result.Y:F6}, y' = {result.DY:F6}");
            Console.WriteLine($"Ожидаемое значение: {Yb}");
            Console.WriteLine($"Разница: {result.Y - Yb:E}\n");

            Console.ResetColor();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        /// <summary>
        /// Отображает заголовок раздела в консоли
        /// </summary>
        private static void DrawHeader(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Спрашивает пользователя, какой метод использовать: RK4 или RK2
        /// </summary>
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

        /// <summary>
        /// Вычисляет правую часть системы ODE: y' = dy, y'' = (exp(x) + y + dy) / 3
        /// </summary>
        private static (double F0, double F1) F(double x, double y, double dy)
        {
            return (dy, (Math.Exp(x) + y + dy) / 3.0);
        }

        /// <summary>
        /// Метод Рунге-Кутта 4-го порядка для системы 2-го порядка
        /// </summary>
        private static (double Y, double DY) RungeKutta4(double a, double b, double h, double y0, double dy0, bool print)
        {
            int n = (int)Math.Round((b - a) / h) + 1;
            double[] X = Enumerable.Range(0, n).Select(i => a + i * h).ToArray();
            double y = y0, dy = dy0;

            if (print)
                Console.WriteLine($"x = {X[0]:F6} | y = {y:F6} | y' = {dy:F6}");

            for (int i = 1; i < n; i++)
            {
                var (k10, k11) = F(X[i - 1], y, dy);
                var (k20, k21) = F(X[i - 1] + h / 2, y + h / 2 * k10, dy + h / 2 * k11);
                var (k30, k31) = F(X[i - 1] + h / 2, y + h / 2 * k20, dy + h / 2 * k21);
                var (k40, k41) = F(X[i - 1] + h, y + h * k30, dy + h * k31);

                y += h / 6 * (k10 + 2 * k20 + 2 * k30 + k40);
                dy += h / 6 * (k11 + 2 * k21 + 2 * k31 + k41);

                if (print)
                    Console.WriteLine($"x = {X[i]:F6} | y = {y:F6} | y' = {dy:F6}");
            }
            return (y, dy);
        }

        /// <summary>
        /// Метод Рунге-Кутта 2-го порядка (метод модифицированного Эйлера)
        /// </summary>
        private static (double Y, double DY) RungeKutta2(double a, double b, double h, double y0, double dy0, bool print)
        {
            int n = (int)Math.Round((b - a) / h) + 1;
            double[] X = Enumerable.Range(0, n).Select(i => a + i * h).ToArray();
            double y = y0, dy = dy0;

            if (print)
                Console.WriteLine($"x = {X[0]:F6} | y = {y:F6} | y' = {dy:F6}");

            for (int i = 1; i < n; i++)
            {
                var (k10, k11) = F(X[i - 1], y, dy);
                var (k20, k21) = F(X[i - 1] + h / 2, y + h / 2 * k10, dy + h / 2 * k11);

                y += h * k20;
                dy += h * k21;

                if (print)
                    Console.WriteLine($"x = {X[i]:F6} | y = {y:F6} | y' = {dy:F6}");
            }
            return (y, dy);
        }

        /// <summary>
        /// Осуществляет метод стрельбы для поиска начального dy по краевому условию
        /// </summary>
        private static double FindInitialSlope(double left, double right, bool useRk4)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Начальный интервал для k: [{left}, {right}]");
            Console.ResetColor();

            double fa = ShotFunction(left, useRk4);
            double fb = ShotFunction(right, useRk4);
            if (fa * fb >= 0)
                throw new InvalidOperationException("f(a) и f(b) должны иметь разные знаки!");

            double mid = 0;
            int iter = 0;
            while (Math.Abs(right - left) > Epsilon)
            {
                iter++;
                mid = (left + right) / 2;
                Console.WriteLine($"\nИтерация {iter}: интервал k = [{left:F8}, {right:F8}], проверяем k = {mid:F8}");
                double fm = ShotFunction(mid, useRk4);
                if (fm == 0) break;
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nНайденный интервал для k: [{left:F8}, {right:F8}]");
            Console.ResetColor();
            return (left + right) / 2;
        }

        /// <summary>
        /// Вычисляет отклонение в точке b для заданного k
        /// </summary>
        private static double ShotFunction(double k, bool useRk4)
        {
            var result = useRk4
                ? RungeKutta4(A, B, Step, Ya, k, false)
                : RungeKutta2(A, B, Step, Ya, k, false);
            double error = result.Y - Yb;
            Console.WriteLine($"k = {k:F8} => y({B}) = {result.Y:F6}, отклонение = {error:E}");
            return error;
        }

        /// <summary>
        /// Подбирает оптимальный шаг h для достижения заданной точности в y(b)
        /// </summary>
        private static double AdjustStep(double eps, double h, double y0, double dy0, bool useRk4)
        {
            const double MinH = 1e-14;
            while (h > MinH)
            {
                var result = useRk4
                    ? RungeKutta4(A, B, h, y0, dy0, false)
                    : RungeKutta2(A, B, h, y0, dy0, false);
                double error = Math.Abs(result.Y - Yb);
                Console.WriteLine($"h = {h:E}, y(b) = {result.Y:F6}, ошибка = {error:E}");
                if (error < eps)
                {
                    Console.WriteLine($"Достигнута точность {eps:E}: оптимальный шаг = {h:E}");
                    return h;
                }
                h /= 2;
            }
            Console.WriteLine("Минимальный шаг достигнут, но точность не достигнута.");
            return h;
        }
    }
}
