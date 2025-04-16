using Shared;

namespace _8
{
    internal class Program
    {
        static Func<double, double> function = x => Math.Pow(x, 2);

        const double xMin = 0.0, xStep = 1, xTarget = 1.5;
        const int xCount = 4;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, xStep, xCount);

        static TrigonometricInterpolation interpolator = new(dataPoints);

        static void Main()
        {
            Printer.PrintListAsTable(dataPoints, "Исходные данные");

            interpolator.Compute(xTarget);
        }
    }
}
