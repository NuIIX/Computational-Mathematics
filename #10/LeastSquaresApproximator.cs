using Shared;

namespace _10
{
    public class LeastSquaresApproximator : Approximation
    {
        private readonly List<Func<double, double>> basisFunctions;

        public LeastSquaresApproximator(List<(double x, double y)> points, List<Func<double, double>> basis)
            : base(points)
        {
            basisFunctions = basis;
        }

        public override Func<double, double> Compute(bool useWrite)
        {
            int n = points.Count;
            int m = basisFunctions.Count;

            double[,] aMatrix = new double[m, m];
            double[] bVector = new double[m];

            if (useWrite) Console.WriteLine("Матрица A (G^T * G):");

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    double sum = 0;

                    foreach (var (x, _) in points)
                    {
                        sum += basisFunctions[i](x) * basisFunctions[j](x);
                    }

                    aMatrix[i, j] = sum;

                    if (useWrite) Console.Write($"{aMatrix[i, j]:F4}\t");
                }

                if (useWrite) Console.WriteLine();
            }

            if (useWrite) Console.WriteLine("\nВектор B (G^T * Y):");

            for (int i = 0; i < m; i++)
            {
                double sum = 0;

                foreach (var (x, y) in points)
                {
                    sum += y * basisFunctions[i](x);
                }

                bVector[i] = sum;
                
                if (useWrite) Console.WriteLine($"B[{i}] = {bVector[i]:F4}");
            }

            var solver = new GaussianElimination(aMatrix, bVector);
            double[] coeffs = solver.Solve();

            if (useWrite)
            {
                Console.WriteLine("\nРешение (коэффициенты):");

                for (int i = 0; i < coeffs.Length; i++)
                {
                    Console.WriteLine($"a{i} = {coeffs[i]:F4}");
                }
            }

            if (useWrite)
            {
                Console.WriteLine("\nАппроксимирующая функция:");
                Console.Write("f(x) = ");

                for (int i = 0; i < coeffs.Length; i++)
                {
                    Console.Write($"{coeffs[i]:F3} * g{i}(x)");
                    if (i < coeffs.Length - 1) Console.Write(" + ");
                }

                Console.WriteLine();
            }

            return x =>
            {
                double result = 0;

                for (int i = 0; i < coeffs.Length; i++)
                {
                    result += coeffs[i] * basisFunctions[i](x);
                }

                return result;
            };
        }
    }
}
