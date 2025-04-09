namespace _4
{
    internal class Program
    {
        static void Main()
        {
            Func<double, double> function = x => x * x - 3, derivative = x => 2 * x;
            const double tolerance = 1e-4;

            RootFindingMethod bisection = new BisectionMethod(function, tolerance);
            double root1 = bisection.Solve(1, 2);
            Console.WriteLine($"\n\tКорень (метод половинного деления): {root1}\n");

            RootFindingMethod chord = new ChordMethod(function, tolerance);
            double root2 = chord.Solve(1, 2);
            Console.WriteLine($"\n\tКорень (метод хорд): {root2}\n");

            NewtonMethod newton = new NewtonMethod(function, derivative, tolerance);
            double root3 = newton.Solve(2);
            Console.WriteLine($"\n\tКорень (метод Ньютона): {root3}");
        }
    }
}
