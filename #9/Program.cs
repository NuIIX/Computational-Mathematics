using _9;
using Shared;

internal class Program
{
    private static void Main(string[] args)
    {
        Func<double, double> function = x => 1 / x;
        const double start = 1, end = 2, step = 0.1;

        Printer.PrintListAsTable(ListUtils.FillDataPoints(function, start, step, Convert.ToInt32((end - start) / step) + 1));

        Console.WriteLine("Метод трапеций");
        var trapezoid = new TrapezoidalCalculator(function, start, end, step);
        double result1 = trapezoid.Compute();

        Console.WriteLine("\nМетод Симпсона");
        var simpson = new SimpsonCalculator(function, start, end, step);
        double result2 = simpson.Compute();

        Console.WriteLine($"\nИтог трапеций: {result1}");
        Console.WriteLine($"Итог Симпсона: {result2}");
    }
}