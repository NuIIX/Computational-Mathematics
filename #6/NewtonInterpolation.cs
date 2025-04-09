namespace _6
{
    public class NewtonInterpolation : InterpolationBase
    {
        protected double[,] _differenceTable;

        public NewtonInterpolation(List<(double x, double y)> dataPoints)
            : base(dataPoints)
        {
            _differenceTable = new double[PointsCount, PointsCount];

            CalculateDifferenceTable();
        }

        private void CalculateDifferenceTable()
        {
            for (int i = 0; i < PointsCount; i++)
            {
                _differenceTable[i, 0] = _points[i].y;
            }

            for (int j = 1; j < PointsCount; j++)
            {
                for (int i = 0; i < PointsCount - j; i++)
                {
                    _differenceTable[i, j] = _differenceTable[i + 1, j - 1] - _differenceTable[i, j - 1];
                }
            }
        }

        protected static double Factorial(int n)
        {
            if (n == 0)
            {
                return 1;
            }

            double result = 1;

            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        public void PrintDifferenceTable()
        {
            int columnWidth = Math.Max("x".Length, "y".Length);

            for (int i = 0; i < PointsCount; i++)
            {
                columnWidth = Math.Max(columnWidth, _points[i].x.ToString("F6").Length);
            }

            for (int i = 0; i < PointsCount; i++)
            {
                columnWidth = Math.Max(columnWidth, _differenceTable[i, 0].ToString("F6").Length);
            }

            string header = "x".PadRight(columnWidth) + "\t" + "y".PadRight(columnWidth);

            for (int i = 1; i < PointsCount; i++)
            {
                header += $"\t{($"d{(i == 1 ? "" : i)}y").PadRight(columnWidth)}";
            }

            Console.WriteLine(header);

            for (int i = 0; i < PointsCount; i++)
            {
                Console.Write($"{_points[i].x.ToString("F6").PadRight(columnWidth)}\t");

                for (int j = 0; j < PointsCount - i; j++)
                {
                    Console.Write($"{_differenceTable[i, j]:F6}".PadRight(columnWidth) + "\t");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
