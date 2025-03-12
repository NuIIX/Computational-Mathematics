namespace _1
{
    internal class GaussianElimination
    {
        protected int size;
        protected double[,] matrix;
        protected double[] results;

        public GaussianElimination(double[,] inputMatrix, double[] inputResults)
        {
            size = inputMatrix.GetLength(0);
            matrix = (double[,])inputMatrix.Clone();
            results = (double[])inputResults.Clone();
        }

        public virtual void ConvertToLowerTriangular()
        {
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    double factor = matrix[j, i] / matrix[i, i];

                    for (int k = i; k < size; k++)
                    {
                        matrix[j, k] -= factor * matrix[i, k];
                    }

                    results[j] -= factor * results[i];
                }
            }
        }

        public void Solve()
        {
            Console.WriteLine("\nИсходная матрица:");
            PrintMatrix();

            ConvertToLowerTriangular();

            Console.WriteLine("\nТреугольная матрица:");
            PrintMatrix();

            double[] solution = BackSubstitution();

            Console.WriteLine("\nРешение:");
            PrintSolution(solution);
        }

        protected double[] BackSubstitution()
        {
            double[] solution = new double[size];

            for (int i = size - 1; i >= 0; i--)
            {
                double sum = results[i];

                for (int j = i + 1; j < size; j++)
                {
                    sum -= matrix[i, j] * solution[j];
                }

                solution[i] = sum / matrix[i, i];
            }

            return solution;
        }

        protected void PrintMatrix()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write($"{matrix[i, j],8:F2} ");
                }

                Console.WriteLine($"| {results[i],8:F2}");
            }
        }

        protected void PrintSolution(double[] solution)
        {
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine($"x{i + 1} = {solution[i]:F4}");
            }
        }
    }

}