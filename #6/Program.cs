using Shared;

namespace _6
{
    internal class Program
    {
        static Func<double, double> function = Math.Sqrt;

        const double xMin = 0.0, xStep = 0.2, xTarget = 0;
        const int xCount = 10;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, xStep, xCount);

        static LagrangeInterpolation lagrangeInterpolator = new(dataPoints);
        static EitkenInterpolation eitkenInterpolator = new(dataPoints);
        static NewtonForwardInterpolation forwardInterpolator = new(dataPoints);
        static NewtonBackwardInterpolation backwardInterpolator = new(dataPoints);

        [STAThread]
        static void Main()
        {
            double resultForward;
            double resultBackward;

            Printer.PrintListAsTable(dataPoints, "Исходная функция:");

            lagrangeInterpolator.SolveAndPrint(xTarget);

            eitkenInterpolator.SolveAndPrint(xTarget);

            Console.WriteLine("\nПервая формула Ньютона:\n");
            Console.WriteLine("Таблица конечных разностей:");
            forwardInterpolator.PrintDifferenceTable();
            resultForward = forwardInterpolator.Compute(xTarget);
            Console.WriteLine($"\nРезультат первой формулы Ньютона: P({xTarget}) = {resultForward:F6}");

            Console.WriteLine("\nВторая формула Ньютона:\n");
            resultBackward = backwardInterpolator.Compute(xTarget);
            Console.WriteLine($"\nРезультат второй формулы Ньютона: P({xTarget}) = {resultBackward:F6}");

            PlotterForms.Program.ShowGraph(GraphGenerator.GenerateData(lagrangeInterpolator, function, xMin, xStep, xCount));
        }
    }
}
