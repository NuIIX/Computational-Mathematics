using Shared;

namespace _4
{
    public abstract class RootFindingMethod : FunctionBase
    {
        public RootFindingMethod(Func<double, double> function, double tolerance = 1e-6)
            : base(function, tolerance: tolerance) { }

        public abstract double Solve(double a, double b);

        protected abstract double FindC(double a, double b);

        protected void PrintStep(int step, double a, double b, double c, double epsilon)
        {
            Console.WriteLine($"\nШаг {step}:");
            Console.WriteLine($"a = {Round(a)}, b = {Round(b)}, c = {Round(c)}");
            Console.WriteLine($"f(c) = {Round(_function(c))}");
            Console.WriteLine($"E = {Round(epsilon)}");
        }
    }
}
