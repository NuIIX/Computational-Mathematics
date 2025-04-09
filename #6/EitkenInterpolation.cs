namespace _6
{
    public class EitkenInterpolation : InterpolationBase
    {
        public EitkenInterpolation(List<(double x, double y)> dataPoints)
            : base(dataPoints) { }

        public double Compute(double x, bool useWrite = true)
        {
            double[,] p = new double[PointsCount, PointsCount];

            for (int i = 0; i < PointsCount; i++)
            {
                p[i, 0] = _points[i].y;
            }

            for (int j = 1; j < PointsCount; j++)
            {
                for (int i = 0; i < PointsCount - j; i++)
                {
                    p[i, j] = ((x - _points[i + j].x) * p[i, j - 1] - (x - _points[i].x) * p[i + 1, j - 1]) / (_points[i].x - _points[i + j].x);

                    if (useWrite) Console.WriteLine($"P[{i},{j}] = (({x} - {_points[i + j].x}) * {p[i, j - 1]:F6} - ({x} - {_points[i]}) * {p[i + 1, j - 1]:F6}) / ({_points[i].x} - {_points[i + j].x}) = {p[i, j]:F6}");
                }
            }

            return p[0, PointsCount - 1];
        }

        public void SolveAndPrint(double x)
        {
            Console.WriteLine("\nЭйткен:\n");

            double result = Compute(x);

            Console.WriteLine($"\nРезультат интерполяции Эйткена: P({x}) = {result:F6}");
        }
    }
}
