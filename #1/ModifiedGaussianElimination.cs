namespace _1
{
    internal class ModifiedGaussianElimination : GaussianElimination
    {
        public ModifiedGaussianElimination(double[,] inputMatrix, double[] inputResults)
            : base(inputMatrix, inputResults) { }

        public override void ConvertToLowerTriangular()
        {
            for (int i = 0; i < size - 1; i++)
            {
                SwapRowsForMaxPivot(i);

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

        private void SwapRowsForMaxPivot(int col)
        {
            int maxRow = col;
            double maxVal = Math.Abs(matrix[col, col]);

            for (int i = col + 1; i < size; i++)
            {
                if (Math.Abs(matrix[i, col]) > maxVal)
                {
                    maxVal = Math.Abs(matrix[i, col]);
                    maxRow = i;
                }
            }

            if (maxRow != col)
            {
                for (int i = 0; i < size; i++)
                {
                    double temp = matrix[col, i];
                    matrix[col, i] = matrix[maxRow, i];
                    matrix[maxRow, i] = temp;
                }

                double tempResult = results[col];
                results[col] = results[maxRow];
                results[maxRow] = tempResult;
            }
        }
    }
}