using System;
using System.Collections.Generic;

namespace AdaptiveRungeKutta
{
    /// <summary>
    /// Entry point for solving ODE using adaptive Runge-Kutta methods.
    /// </summary>
    class Program
    {
        private const double Epsilon = 1e-12;
        private const double InitialStep = 0.1;
        private const double A = 0.0;
        private const double B = 1.0;
        private const double Y0 = 1.0;
        private const double DY0 = 1.0;

        /// <summary>
        /// Starts the application and runs adaptive RK4 and RK2 solvers.
        /// </summary>
        static void Main(string[] args)
        {
            WriteHeader("Решение ДУ методом Рунге-Кутта с адаптивным шагом");
            WriteEquationInfo();

            WriteSubheader("Адаптация шага для RK4");
            double h4 = AdaptStep(Epsilon, InitialStep, useRK4: true);

            WriteSubheader("Решение методом RK4");
            SolveRungeKutta(A, B, h4, Y0, DY0, useRK4: true, verbose: false);

            WriteSubheader("Адаптация шага для RK2");
            double h2 = AdaptStep(Epsilon, InitialStep, useRK4: false);

            WriteSubheader("Решение методом RK2");
            SolveRungeKutta(A, B, h2, Y0, DY0, useRK4: false, verbose: false);

            Console.ResetColor();
        }

        /// <summary>
        /// Computes derivatives for system: y' = v, v' = (e^x + y + v) / 3.
        /// </summary>
        static double[] Derivatives(double x, double y, double v)
        {
            return new double[] { v, (Math.Exp(x) + y + v) / 3.0 };
        }

        /// <summary>
        /// Solves the ODE system with either RK4 or RK2 method.
        /// </summary>
        static (double y, double v) SolveRungeKutta(double a, double b, double h, double y0, double v0, bool useRK4, bool verbose = false)
        {
            int steps = (int)Math.Round((b - a) / h);
            double x = a, y = y0, v = v0;

            if (verbose) PrintState(0, x, y, v);

            for (int i = 1; i <= steps; i++)
            {
                (y, v) = useRK4 ? RK4Step(x, y, v, h) : RK2Step(x, y, v, h);
                x = a + i * h;
                if (verbose) PrintState(i, x, y, v);
            }

            return (y, v);
        }

        /// <summary>
        /// Performs one RK4 step.
        /// </summary>
        static (double yNext, double vNext) RK4Step(double x, double y, double v, double h)
        {
            var k1 = Derivatives(x, y, v);
            var k2 = Derivatives(x + h / 2, y + h / 2 * k1[0], v + h / 2 * k1[1]);
            var k3 = Derivatives(x + h / 2, y + h / 2 * k2[0], v + h / 2 * k2[1]);
            var k4 = Derivatives(x + h, y + h * k3[0], v + h * k3[1]);

            double yNext = y + h / 6 * (k1[0] + 2 * k2[0] + 2 * k3[0] + k4[0]);
            double vNext = v + h / 6 * (k1[1] + 2 * k2[1] + 2 * k3[1] + k4[1]);
            return (yNext, vNext);
        }

        /// <summary>
        /// Performs one RK2 (Heun) step.
        /// </summary>
        static (double yNext, double vNext) RK2Step(double x, double y, double v, double h)
        {
            var k1 = Derivatives(x, y, v);
            var k2 = Derivatives(x + h / 2, y + h / 2 * k1[0], v + h / 2 * k1[1]);

            double yNext = y + h * k2[0];
            double vNext = v + h * k2[1];
            return (yNext, vNext);
        }

        /// <summary>
        /// Adjusts the step size until the solution meets the error tolerance.
        /// </summary>
        static double AdaptStep(double eps, double hInit, bool useRK4)
        {
            double h = hInit, err;
            int iter = 0;

            do
            {
                iter++;
                var coarse = SolveRungeKutta(A, B, h, Y0, DY0, useRK4);
                var fine = SolveRungeKutta(A, B, h / 2, Y0, DY0, useRK4);
                err = Math.Abs(coarse.y - fine.y);
                PrintAdaptInfo(iter, err, h);
                h /= 2;
            } while (err > eps);

            PrintSuccess($"Оптимальный шаг: {h:F6}");
            return h;
        }

        /// <summary>
        /// Prints a formatted header in the console.
        /// </summary>
        static void WriteHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"=== {title} ===");
            Console.WriteLine(new string('=', 60));
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a subheader in the console.
        /// </summary>
        static void WriteSubheader(string subtitle)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"-- {subtitle} --");
            Console.ResetColor();
        }

        /// <summary>
        /// Displays equation and initial conditions.
        /// </summary>
        static void WriteEquationInfo()
        {
            Console.WriteLine($"Уравнение: y'' = (e^x + y + y')/3, y({A})={Y0}, y'({A})={DY0}");
        }

        /// <summary>
        /// Prints the state at each step in a table-like format.
        /// </summary>
        static void PrintState(int step, double x, double y, double v)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"│ {step,3} │ x={x,6:F4} │ y={y,8:F6} │ y'={v,8:F6} │");
            Console.ResetColor();
        }

        /// <summary>
        /// Prints adaptation iteration info.
        /// </summary>
        static void PrintAdaptInfo(int iteration, double error, double step)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[Iter {iteration}] Error={error,8:F6}, Step={step,8:F6}");
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a success message in the console.
        /// </summary>
        static void PrintSuccess(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
