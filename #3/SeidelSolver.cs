namespace _3
{
    public class SeidelSolver : SimpleIterationSolver
    {
        public SeidelSolver(double[,] matrix, double[] results, double epsilon = 1e-4, int maxIterations = 100)
            : base(matrix, results, epsilon, maxIterations) { }

        public override void Solve()
        {
            int size = _coefficientsMatrix.GetLength(0);
            double[] currentSolution = new double[size];

            PrintMatrix(_iterationMatrix, "\nМатрица итераций C:");
            PrintVector(_iterationVector, "\nВектор правой части B:");
            PrintVector(currentSolution, "\nНачальный вектор x:");

            for (int iteration = 0; iteration < _maxIterations; iteration++)
            {
                double[] previousSolution = (double[])currentSolution.Clone();

                for (int i = 0; i < size; i++)
                {
                    double sum = _iterationVector[i];

                    for (int j = 0; j < size; j++)
                    {
                        if (i != j)
                        {
                            sum += _iterationMatrix[i, j] * currentSolution[j];
                        }
                    }

                    currentSolution[i] = sum;
                }

                PrintSolutionStep(iteration, currentSolution);

                if (HasConverged(previousSolution, currentSolution))
                {
                    PrintVector(currentSolution, "\nРешение найдено:");

                    return;
                }
            }

            Console.WriteLine("\nДостигнуто максимальное число итераций.");
        }
    }
}
