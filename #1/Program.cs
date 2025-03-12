namespace _1
{
    internal class Program
    {
        static void Main()
        {
            double[,] matrix = {
                { 2, -1, 1 },
                { 3,  3, 9 },
                { 3,  3, 5 }
            };

            double[] results = { 2, -1, 4 };

            Console.WriteLine("Обычный метод Гаусса");
            GaussianElimination solver = new GaussianElimination(matrix, results);
            solver.Solve();

            Console.WriteLine("\nМодифицированный метод Гаусса (с выбором главного элемента)");
            ModifiedGaussianElimination modSolver = new ModifiedGaussianElimination(matrix, results);
            modSolver.Solve();
        }
    }
}