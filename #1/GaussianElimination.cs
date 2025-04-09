namespace _1
{
    public class GaussianElimination
    {
        protected int _size;
        protected double[,] _matrix;
        protected double[] _results;

        public GaussianElimination(double[,] inputMatrix, double[] inputResults)
        {
            _size = inputMatrix.GetLength(0);
            _matrix = (double[,])inputMatrix.Clone();
            _results = (double[])inputResults.Clone();
        }

        public virtual void ConvertToLowerTriangular()
        {
            for (int i = 0; i < _size - 1; i++)
            {
                for (int j = i + 1; j < _size; j++)
                {
                    double factor = _matrix[j, i] / _matrix[i, i];

                    for (int k = i; k < _size; k++)
                    {
                        _matrix[j, k] -= factor * _matrix[i, k];
                    }

                    _results[j] -= factor * _results[i];
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
            double[] solution = new double[_size];

            for (int i = _size - 1; i >= 0; i--)
            {
                double sum = _results[i];

                for (int j = i + 1; j < _size; j++)
                {
                    sum -= _matrix[i, j] * solution[j];
                }

                solution[i] = sum / _matrix[i, i];
            }

            return solution;
        }

        protected void PrintMatrix()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Console.Write($"{_matrix[i, j],8:F2} ");
                }

                Console.WriteLine($"| {_results[i],8:F2}");
            }
        }

        protected void PrintSolution(double[] solution)
        {
            for (int i = 0; i < _size; i++)
            {
                Console.WriteLine($"x{i + 1} = {solution[i]:F4}");
            }
        }
    }
}