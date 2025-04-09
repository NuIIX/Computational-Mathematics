namespace _5
{
    internal class Program
    {
        static void Main()
        {
            Func<double[], double[]> functions = x => new double[]
            {
                Math.Pow(x[0], 2) + Math.Pow(x[1], 3) - 4,
                x[0] / x[1] - 2
            };

            Func<double[], double[,]> jacobian = x => new double[,]
            {
                { 2 * x[0], 3 * Math.Pow(x[1], 2) },
                { 1 / x[1], -x[0] / Math.Pow(x[1], 2) }
            };

            double[] initialGuess = { 2, 1 };

            NewtonMethodInverse solver = new NewtonMethodGaussian(functions, jacobian);
            solver.Solve(initialGuess);
        }
    }
}
