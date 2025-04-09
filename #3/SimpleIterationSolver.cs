namespace _3
{
    public class SimpleIterationSolver
    {
        protected double[,] _coefficientsMatrix; 
        protected double[] _constantsVector;
        protected double[,] _iterationMatrix;
        protected double[] _iterationVector;
        protected double _epsilon;
        protected int _maxIterations;

        public SimpleIterationSolver(double[,] matrix, double[] results, double epsilon = 1e-4, int maxIterations = 100)
        {
            int size = matrix.GetLength(0);
            _coefficientsMatrix = matrix;
            _constantsVector = results;
            _epsilon = epsilon;
            _maxIterations = maxIterations;

            _iterationMatrix = new double[size, size];
            _iterationVector = new double[size];

            ComputeIterationParameters();
        }

        private void ComputeIterationParameters()
        {
            int size = _coefficientsMatrix.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                double diagonalElement = _coefficientsMatrix[i, i];

                for (int j = 0; j < size; j++)
                {
                    _iterationMatrix[i, j] = i == j ? 1 : _coefficientsMatrix[i, j] / diagonalElement;
                }

                _iterationVector[i] = _constantsVector[i] / diagonalElement;
            }
        }

        public virtual void Solve()
        {
            int size = _coefficientsMatrix.GetLength(0);
            double[] previousSolution = new double[size];
            double[] currentSolution = new double[size];

            PrintMatrix(_iterationMatrix, "\nМатрица итераций C:");
            PrintVector(_iterationVector, "\nВектор правой части B:");
            PrintVector(previousSolution, "\nНачальный вектор x:");

            for (int iteration = 0; iteration < _maxIterations; iteration++)
            {
                for (int i = 0; i < size; i++)
                {
                    currentSolution[i] = _iterationVector[i];

                    for (int j = 0; j < size; j++)
                    {
                        if (i != j)
                        {
                            currentSolution[i] += _iterationMatrix[i, j] * previousSolution[j];
                        }
                    }
                }

                PrintSolutionStep(iteration, currentSolution);

                if (HasConverged(previousSolution, currentSolution))
                {
                    PrintVector(currentSolution, "\nРешение найдено:");

                    return;
                }

                Array.Copy(currentSolution, previousSolution, size);
            }

            Console.WriteLine("\nДостигнуто максимальное число итераций.");
        }

        protected bool HasConverged(double[] oldSolution, double[] newSolution)
        {
            for (int i = 0; i < oldSolution.Length; i++)
            {
                if (Math.Abs(newSolution[i] - oldSolution[i]) > _epsilon)
                {
                    return false;
                }
            }

            return true;
        }

        protected void PrintSolutionStep(int iteration, double[] solution)
        {
            Console.WriteLine($"\nШаг {iteration + 1}:");
            PrintVector(solution);
        }

        protected void PrintVector(double[] vector, string header = "")
        {
            Console.WriteLine(header);

            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine($"x[{i}] = {vector[i]:F6}");
            }
        }

        protected void PrintMatrix(double[,] matrix, string header = "")
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            Console.WriteLine(header);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],8:F4} ");
                }

                Console.WriteLine();
            }
        }
    }
}
