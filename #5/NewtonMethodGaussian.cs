using Shared;

namespace _5
{
    public class NewtonMethodGaussian : NewtonMethodInverse
    {
        public NewtonMethodGaussian(Func<double[], double[]> functions, Func<double[], double[,]> jacobian, double tolerance = 1e-6)
            : base(functions, jacobian, tolerance) { }

        private double[] SolveLinearSystem(double[,] matrix, double[] values)
        {
            GaussianElimination solver = new GaussianElimination(matrix, values);
            return solver.Solve();
        }

        public override void Solve(double[] initialGuess, int maxIterations = 10)
        {
            double[] x = (double[])initialGuess.Clone();
            double[] prevX = new double[x.Length];

            Console.WriteLine($"Начальное приближение: ({Round(x[0])}, {Round(x[1])})\n");

            for (int iter = 0; iter < maxIterations; iter++)
            {
                Console.WriteLine($"Шаг {iter + 1}:");

                double[] F = _functions(x);
                Console.WriteLine($"F(x^{iter}) = ({Round(F[0])}, {Round(F[1])})");

                double[,] W = _jacobian(x);
                Console.WriteLine($"W(x^{iter}) = \n[{Round(W[0, 0])}, {Round(W[0, 1])}]\n[{Round(W[1, 0])}, {Round(W[1, 1])}]\n");

                double[] y = SolveLinearSystem(W, F);
                Console.WriteLine($"y^{iter} = ({Round(y[0])}, {Round(y[1])})");

                prevX[0] = x[0];
                prevX[1] = x[1];

                x[0] -= y[0];
                x[1] -= y[1];

                double[] epsilon =
                {
                    Math.Abs(x[0] - prevX[0]),
                    Math.Abs(x[1] - prevX[1])
                };

                Console.WriteLine($"x^{iter + 1} = ({Round(x[0])}, {Round(x[1])})");
                Console.WriteLine($"E = ({epsilon[0]}, {epsilon[1]})\n");

                if (epsilon[0] < _tolerance && epsilon[1] < _tolerance)
                {
                    Console.WriteLine($"Решение найдено: ({Round(x[0])}, {Round(x[1])})");
                    return;
                }
            }

            Console.WriteLine("Метод не сошелся за указанное число итераций.");
        }
    }

}
