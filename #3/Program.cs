namespace _3
{
    internal class Program
    {
        static void Main()
        {
            double[,] coefficientsMatrix = {
                { 5, -1, 2 },
                { -2, -10, 3 },
                { 1, 2, 5 }
            };

            double[] constantsVector = { 3, -4, 12 };

            SimpleIterationSolver solver = new SeidelSolver(coefficientsMatrix, constantsVector, 1e-4);
            solver.Solve();
        }
    }
}
