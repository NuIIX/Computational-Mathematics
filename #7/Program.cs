using Shared;

namespace _7
{
    internal class Program
    {
        static Func<double, double> function = Math.Sqrt;

        const double xMin = 0.0, xStep = 0.2, xTarget = 2.56;
        const int xCount = 10;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, xStep, xCount);

        static CubicSplineInterpolation csInterpolator = new(dataPoints);

        [STAThread]
        static void Main()
        {
            csInterpolator.Compute(xTarget);

            PlotterForms.Program.ShowGraph(GraphGenerator.GenerateData(csInterpolator, function, xMin, xStep, xCount));
        }
    }
}