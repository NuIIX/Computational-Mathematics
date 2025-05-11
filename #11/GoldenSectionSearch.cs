namespace _11
{
    public class GoldenSectionSearch
    {
        private readonly Func<double, double> _function;
        private readonly double _aStart;
        private readonly double _bStart;
        private readonly int _steps;

        private const double Phi1 = 0.382;
        private const double Phi2 = 0.618;

        public GoldenSectionSearch(Func<double, double> function, double a, double b, int steps = 4)
        {
            _function = function;
            _aStart = a;
            _bStart = b;
            _steps = steps;
        }

        public double Compute(bool useWrite = true)
        {
            double a = _aStart, b = _bStart;

            if (useWrite) Console.WriteLine($"Интервал: [{a}, {b}], шагов: {_steps}");

            for (int i = 1; i <= _steps; i++)
            {
                double lambda1 = a + Phi1 * (b - a);
                double lambda2 = a + Phi2 * (b - a);

                double f1 = _function(lambda1);
                double f2 = _function(lambda2);

                if (useWrite)
                {
                    Console.WriteLine($"\nИтерация {i}");
                    Console.WriteLine($"л1 = {lambda1:F6}");
                    Console.WriteLine($"л2 = {lambda2:F6}");
                    Console.WriteLine($"f(л1) = {f1:F6}");
                    Console.WriteLine($"f(л2) = {f2:F6}");
                }

                if (f1 > f2)
                {
                    if (useWrite) Console.WriteLine("f(л1) > f(л2), значит a = л1");
                    a = lambda1;
                }
                else
                {
                    if (useWrite) Console.WriteLine("f(л1) < f(л2), значит b = л2");
                    b = lambda2;
                }
            }

            double x = (a + b) / 2;
            double fx = _function(x);

            if (useWrite)
            {
                Console.WriteLine("\nРезультаты вычислений:");
                Console.WriteLine($"x = ({a:F6} + {b:F6}) / 2 = {x:F6}");
                Console.WriteLine($"f(x) = {fx:F6}");
            }

            return x;
        }
    }
}
