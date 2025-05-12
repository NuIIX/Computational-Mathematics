using System;
using System.Collections.Generic;

namespace AdaptiveRungeKutta
{
    class Program
    {
        // Parameters
        private const double Epsilon = 1e-5;
        private const double InitialStep = 0.1;
        private const double A = 0.0;
        private const double B = 1.0;
        private const double Y0 = 1.0;
        private const double DY0 = 1.0;

        static void Main(string[] args)
        {
            Console.WriteLine("===== Решение ДУ методом Рунге-Кутта с адаптивным шагом =====");
            Console.WriteLine($"Уравнение: y'' = (e^x + y + y')/3, y({A}) = {Y0}, y'({A}) = {DY0}");

            // Подбор шага для метода 4 порядка
            Console.WriteLine("\n=== Адаптация шага для RK4 ===");
            double h4 = AdaptStep(Epsilon, InitialStep, useRK4: true);

            Console.WriteLine("\n=== Решение методом RK4 ===");
            SolveRungeKutta(A, B, h4, Y0, DY0, useRK4: true, verbose: true);

            // Подбор шага для метода 2 порядка
            Console.WriteLine("\n=== Адаптация шага для RK2 ===");
            double h2 = AdaptStep(Epsilon, InitialStep, useRK4: false);

            Console.WriteLine("\n=== Решение методом RK2 ===");
            SolveRungeKutta(A, B, h2, Y0, DY0, useRK4: false, verbose: true);
        }

        // Функция правой части системы: y' = v, v' = (e^x + y + v)/3
        static double[] Derivatives(double x, double y, double v)
        {
            return new double[]
            {
                v,
                (Math.Exp(x) + y + v) / 3.0
            };
        }

        // Общий метод решения РК2 или РК4
        static (double y, double v) SolveRungeKutta(double a, double b, double h, double y0, double v0, bool useRK4, bool verbose = false)
        {
            int steps = (int)Math.Round((b - a) / h);
            double x = a;
            double y = y0;
            double v = v0;

            if (verbose)
                PrintState(0, x, y, v);

            for (int i = 1; i <= steps; i++)
            {
                if (useRK4)
                {
                    (y, v) = RK4Step(x, y, v, h);
                }
                else
                {
                    (y, v) = RK2Step(x, y, v, h);
                }

                x = a + i * h;
                if (verbose)
                    PrintState(i, x, y, v);
            }
            return (y, v);
        }

        // Один шаг метода Рунге-Кутта 4-го порядка
        static (double yNext, double vNext) RK4Step(double x, double y, double v, double h)
        {
            double[] k1 = Derivatives(x, y, v);
            double[] k2 = Derivatives(x + h / 2, y + h / 2 * k1[0], v + h / 2 * k1[1]);
            double[] k3 = Derivatives(x + h / 2, y + h / 2 * k2[0], v + h / 2 * k2[1]);
            double[] k4 = Derivatives(x + h, y + h * k3[0], v + h * k3[1]);

            double yNext = y + h / 6 * (k1[0] + 2 * k2[0] + 2 * k3[0] + k4[0]);
            double vNext = v + h / 6 * (k1[1] + 2 * k2[1] + 2 * k3[1] + k4[1]);
            return (yNext, vNext);
        }

        // Один шаг метода Рунге-Кутта 2-го порядка (метод Хойна)
        static (double yNext, double vNext) RK2Step(double x, double y, double v, double h)
        {
            double[] k1 = Derivatives(x, y, v);
            double[] k2 = Derivatives(x + h / 2, y + h / 2 * k1[0], v + h / 2 * k1[1]);

            double yNext = y + h * k2[0];
            double vNext = v + h * k2[1];
            return (yNext, vNext);
        }

        // Адаптивный подбор шага с двойным пересчетом
        static double AdaptStep(double eps, double hInit, bool useRK4)
        {
            double h = hInit;
            double err;
            int iteration = 0;

            do
            {
                iteration++;
                var coarse = SolveRungeKutta(A, B, h, Y0, DY0, useRK4, verbose: false);
                var fine = SolveRungeKutta(A, B, h / 2, Y0, DY0, useRK4, verbose: false);
                err = Math.Abs(coarse.y - fine.y);
                Console.WriteLine($"Итерация {iteration}: ошибка = {err:F6}, шаг = {h:F6}");
                h /= 2;
            }
            while (err > eps);

            double optimized = h;
            Console.WriteLine($"Достигнута точность {eps}. Оптимальный шаг: {optimized:F6}");
            return optimized;
        }

        // Вывод состояния в консоль
        static void PrintState(int step, double x, double y, double v)
        {
            Console.WriteLine($"Шаг {step,3}: x = {x,7:F4} | y = {y,10:F6} | y' = {v,10:F6}");
        }
    }
}
