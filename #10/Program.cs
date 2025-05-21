using Shared;

namespace _10
{
    internal class Program
    {
        static Func<double, double> function = x => Math.Sin(x);
        static List<Func<double, double>> basis = new()
        {
            x => 1,
            x => x,
            Math.Sqrt
        };

        const double xMin = 0.0, xStep = 1.0;
        const int xCount = 4;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, xStep, xCount);

        private static void Main(string[] args)
        {
            Printer.PrintListAsTable(dataPoints, "Исходные данные для аппроксимации");

            Approximation approximation = new LeastSquaresApproximator(dataPoints, basis);
            var approximatedFunction = approximation.Compute();

            double plotXMin = dataPoints.Min(p => p.x) - xStep * 0.5;
            double plotXMax = dataPoints.Max(p => p.x) + xStep * 0.5;

            var graphData = GraphGenerator.GenerateApproximationData(
                function,
                approximatedFunction,
                dataPoints,
                plotXMin,
                plotXMax
            );

            PlotterForms.Program.ShowGraph(graphData);
        }
    }
}
