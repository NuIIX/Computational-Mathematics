using System;

namespace NewtonSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Func<double[], double[]> functionEvaluator = x => new double[]
            {
                Math.Pow(x[0], 2) + Math.Pow(x[1], 3) - 4,
                x[0] / x[1] - 2
            };

            Func<double[], double[,]> jacobianEvaluator = x => new double[,]
            {
                { 2 * x[0], 3 * Math.Pow(x[1], 2) },
                { 1 / x[1], -x[0] / Math.Pow(x[1], 2) }
            };

            double[] initialGuess = { 2, 1 };

            var solver = new NewtonInverseSolver(functionEvaluator, jacobianEvaluator);
            solver.Solve(initialGuess);

            Console.WriteLine("\nНажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}
