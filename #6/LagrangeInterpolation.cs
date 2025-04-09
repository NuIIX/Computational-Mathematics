namespace _6
{
    public class LagrangeInterpolation : InterpolationBase
    {
        public LagrangeInterpolation(List<(double x, double y)> dataPoints)
            : base(dataPoints) { }

        public double Compute(double x, bool useWrite = true)
        {
            double result = 0;

            for (int i = 0; i < PointsCount; i++)
            {
                double term = _points[i].y;

                for (int j = 0; j < PointsCount; j++)
                {
                    if (i != j)
                    {
                        term *= (x - _points[j].x) / (_points[i].x- _points[j].x);
                        if (useWrite) Console.Write($"(x - {_points[j].x}) / ({_points[i].x} - {_points[j].x}) * ");
                    }
                }

                result += term;
                if (useWrite) Console.WriteLine($"{_points[i].y} -> {term}");
            }

            return result;
        }

        public double ComputeTruncationError(double x)
        {
            double maxDerivative = 15.0 / 16;
            double factorial = 24.0;
            double product = 1;

            foreach (var (xi, _) in _points)
            {
                product *= (x - xi);
            }

            return Math.Abs((maxDerivative / factorial) * product);
        }

        public void SolveAndPrint(double x)
        {
            Console.WriteLine("\nЛагранж:\n");

            double interpolatedValue = Compute(x);
            double truncationError = ComputeTruncationError(x);
            double roundError = 5e-5;
            double realError = truncationError + roundError;

            Console.WriteLine($"Значение в x = {x}: {interpolatedValue:F4}");
            Console.WriteLine($"Погрешность усечения: {truncationError:F6}");
            Console.WriteLine($"Погрешность округления: {roundError:F6}");
            Console.WriteLine($"Реальная погрешность: {realError:F6}");
        }
    }
}
