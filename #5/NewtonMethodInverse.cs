using Shared;

namespace _5
{
    public class NewtonMethodInverse : PrecisionSettings
    {
        protected readonly Func<double[], double[]> _functions;
        protected readonly Func<double[], double[,]> _jacobian;

        public NewtonMethodInverse(Func<double[], double[]> functions, Func<double[], double[,]> jacobian, double tolerance = 1e-6)
            : base(tolerance)
        {
            _functions = functions;
            _jacobian = jacobian;
        }

        virtual public void Solve(double[] initialGuess, int maxIterations = 10)
        {
            double[] x = (double[])initialGuess.Clone();
            double[] prevX = new double[x.Length];

            Console.WriteLine($"Начальное приближение: ({Round(x[0])}, {Round(x[1])})\n");

            for (int iter = 0; iter < maxIterations; iter++)
            {
                Console.WriteLine($"Шаг {iter + 1}:");

                double[] F = _functions(x);
                Console.WriteLine($"F(x^{iter}) = ({Round(F[0])}, {Round(F[1])})");

                double[,] w = _jacobian(x);
                Console.WriteLine($"W(x^{iter}) = \n[{Round(w[0, 0])}, {Round(w[0, 1])}]\n[{Round(w[1, 0])}, {Round(w[1, 1])}]\n");

                double det = w[0, 0] * w[1, 1] - w[0, 1] * w[1, 0];

                if (Math.Abs(det) < _tolerance)
                {
                    Console.WriteLine("Якобиан вырожден, метод не может продолжаться.");
                    return;
                }

                double[,] wInv = new double[2, 2]
                {
                    { w[1, 1] / det, -w[0, 1] / det },
                    { -w[1, 0] / det, w[0, 0] / det }
                };

                Console.WriteLine($"W^(-1)(x^{iter}) = \n[{Round(wInv[0, 0])}, {Round(wInv[0, 1])}]\n[{Round(wInv[1, 0])}, {Round(wInv[1, 1])}]\n");

                double[] deltaX = 
                {
                    wInv[0, 0] * F[0] + wInv[0, 1] * F[1],
                    wInv[1, 0] * F[0] + wInv[1, 1] * F[1]
                };

                prevX[0] = x[0];
                prevX[1] = x[1];

                x[0] -= deltaX[0];
                x[1] -= deltaX[1];

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
