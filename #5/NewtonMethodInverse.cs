using System;

namespace NewtonSolver
{
    public class NewtonInverseSolver : FunctionBase
    {
        private readonly Func<double[], double[]> _functionEvaluator;
        private readonly Func<double[], double[,]> _jacobianEvaluator;

        public NewtonInverseSolver(Func<double[], double[]> functionEvaluator,
                                   Func<double[], double[,]> jacobianEvaluator,
                                   double tolerance = 1e-6)
            : base(tolerance)
        {
            _functionEvaluator = functionEvaluator;
            _jacobianEvaluator = jacobianEvaluator;
        }

        public void Solve(double[] initialGuess, int maxIterations = 10)
        {
            double[] currentEstimate = (double[])initialGuess.Clone();

            WriteColoredLine($"Начальное приближение: ({Format(currentEstimate[0])}, {Format(currentEstimate[1])})", ConsoleColor.Cyan);
            WriteColoredLine(new string('-', 50));

            for (int iter = 0; iter < maxIterations; iter++)
            {
                WriteColoredLine($"Итерация {iter + 1}:", ConsoleColor.Yellow);

                double[] functionValues = _functionEvaluator(currentEstimate);
                WriteColoredLine($"F(x{iter}) = ({Format(functionValues[0])}, {Format(functionValues[1])})", ConsoleColor.Green);

                double[,] jacobianMatrix = _jacobianEvaluator(currentEstimate);
                WriteColoredLine("Jacobian matrix:", ConsoleColor.Magenta);
                WriteColoredLine($"[{Format(jacobianMatrix[0, 0])}, {Format(jacobianMatrix[0, 1])}]", ConsoleColor.Magenta);
                WriteColoredLine($"[{Format(jacobianMatrix[1, 0])}, {Format(jacobianMatrix[1, 1])}]", ConsoleColor.Magenta);

                double det = jacobianMatrix[0, 0] * jacobianMatrix[1, 1] - jacobianMatrix[0, 1] * jacobianMatrix[1, 0];
                if (Math.Abs(det) < Tolerance)
                {
                    WriteColoredLine("Якобиан вырожден. Метод прерван.", ConsoleColor.Red);
                    return;
                }

                double[,] inverseJacobian = new double[2, 2]
                {
                    { jacobianMatrix[1, 1] / det, -jacobianMatrix[0, 1] / det },
                    { -jacobianMatrix[1, 0] / det, jacobianMatrix[0, 0] / det }
                };

                WriteColoredLine("Обратная матрица Якобиана:", ConsoleColor.Magenta);
                WriteColoredLine($"[{Format(inverseJacobian[0, 0])}, {Format(inverseJacobian[0, 1])}]", ConsoleColor.Magenta);
                WriteColoredLine($"[{Format(inverseJacobian[1, 0])}, {Format(inverseJacobian[1, 1])}]", ConsoleColor.Magenta);

                double[] delta = new double[2]
                {
                    inverseJacobian[0, 0] * functionValues[0] + inverseJacobian[0, 1] * functionValues[1],
                    inverseJacobian[1, 0] * functionValues[0] + inverseJacobian[1, 1] * functionValues[1]
                };

                currentEstimate[0] -= delta[0];
                currentEstimate[1] -= delta[1];

                WriteColoredLine($"Новый приближенный корень: ({Format(currentEstimate[0])}, {Format(currentEstimate[1])})", ConsoleColor.Cyan);
                WriteColoredLine(new string('-', 50));

                if (Math.Abs(delta[0]) < Tolerance && Math.Abs(delta[1]) < Tolerance)
                {
                    WriteColoredLine($"Решение найдено: ({Format(currentEstimate[0])}, {Format(currentEstimate[1])})", ConsoleColor.Green);
                    return;
                }
            }

            WriteColoredLine("Метод не сошелся за заданное количество итераций.", ConsoleColor.Red);
        }

        private void WriteColoredLine(string message, ConsoleColor color = ConsoleColor.White)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
    }
}
