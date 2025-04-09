using PlotterForms;
using Shared;

namespace _6
{
    internal class Program
    {
        static Func<double, double> function = Math.Sqrt;

        const double xMin = 1.0, step = 1.0, xTarget = 2.56;
        const int xCount = 4;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, step, xCount);

        static LagrangeInterpolation? lagrangeInterpolator;
        static EitkenInterpolation? eitkenInterpolator;
        static NewtonForwardInterpolation? forwardInterpolation;
        static NewtonBackwardInterpolation? backwardInterpolation;

        [STAThread]
        static void Main()
        {
            double resultForward;
            double resultBackward;

            Printer.PrintListAsTable(dataPoints, "Исходная функция:");

            lagrangeInterpolator = new(dataPoints);
            eitkenInterpolator = new(dataPoints);
            forwardInterpolation = new(dataPoints);
            backwardInterpolation = new(dataPoints);

            lagrangeInterpolator.SolveAndPrint(xTarget);

            eitkenInterpolator.SolveAndPrint(xTarget);

            Console.WriteLine("\nПервая формула Ньютона:\n");
            Console.WriteLine("Таблица конечных разностей:");
            forwardInterpolation.PrintDifferenceTable();
            resultForward = forwardInterpolation.Compute(xTarget);
            Console.WriteLine($"\nРезультат первой формулы Ньютона: P({xTarget}) = {resultForward:F6}");

            Console.WriteLine("\nВторая формула Ньютона:\n");
            resultBackward = backwardInterpolation.Compute(xTarget);
            Console.WriteLine($"\nРезультат второй формулы Ньютона: P({xTarget}) = {resultBackward:F6}");

            GenerateGraph();
        }

        static void GenerateGraph()
        {
            List<(double x, double y)> xyValues = new();
            const double axisLimit = 100, step = 0.5;

            for (double x = -axisLimit; x <= axisLimit; x += step)
            {
                double y = lagrangeInterpolator!.Compute(x, false);
                xyValues.Add((x, y));
            }

            double minX = dataPoints.Min(p => p.x);
            double maxX = dataPoints.Max(p => p.x);

            List<(double x, double y)> limitsLeft = new()
            {
                (minX, axisLimit),
                (minX, -axisLimit)
            };

            List<(double x, double y)> limitsRight = new()
            {
                (maxX, axisLimit),
                (maxX, -axisLimit)
            };

            List<GraphParameters> graphData = new()
            {
                new GraphParameters(xyValues, markerSize: 0, lineWidth: 3, color: Color.Purple, label: "Линия интерполяции"),
    
                new GraphParameters(dataPoints, markerSize: 0, lineWidth: 3, color: Color.Green, label: "Исходные данные"),
    
                new GraphParameters(limitsLeft, markerSize: 0, lineWidth: 1, color: Color.Gray),
                new GraphParameters(limitsRight, markerSize: 0, lineWidth: 1, color: Color.Gray),
    
                new GraphParameters(dataPoints, markerSize: 10, lineWidth: 0, color: Color.Black, label: "Ключевые точки")
            };

            PlotterForms.Program.ShowGraph(graphData);
        }
    }
}
