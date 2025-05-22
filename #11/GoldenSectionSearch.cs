namespace _11
{
    public class GoldenSectionSearch
    {
        private readonly Func<double, double> _function;
        private double _a;
        private double _b;
        private readonly int _maxSteps;
        private readonly double _epsilon;
        private readonly bool _useEpsilon;

        private const double Phi1 = 0.3819660113;
        private const double Phi2 = 0.6180339887;

        public GoldenSectionSearch(Func<double, double> function, double a, double b, int steps)
        {
            _function = function;
            _a = a;
            _b = b;
            _maxSteps = steps;
            _epsilon = 0;
            _useEpsilon = false;
        }

        public GoldenSectionSearch(Func<double, double> function, double a, double b, double epsilon)
        {
            _function = function;
            _a = a;
            _b = b;
            _epsilon = epsilon;
            _maxSteps = int.MaxValue;
            _useEpsilon = true;
        }

        public double Compute(bool useWrite = true)
        {
            double a = _a;
            double b = _b;

            if (useWrite)
            {
                Console.WriteLine($"Интервал: [{a}, {b}]");
                Console.WriteLine(_useEpsilon
                    ? $"Точность eps = {_epsilon:G6}"
                    : $"Максимум шагов = {_maxSteps}");
            }

            double lambda1 = a + Phi1 * (b - a);
            double lambda2 = a + Phi2 * (b - a);
            double f1 = _function(lambda1);
            double f2 = _function(lambda2);

            int iteration = 0;

            while (iteration < _maxSteps && (!_useEpsilon || (b - a)/2 > _epsilon))
            {
                iteration++;
                if (useWrite)
                {
                    Console.WriteLine($"\nИтерация {iteration}");
                    Console.WriteLine($"л1 = {lambda1:F8}, f(л1) = {f1:F8}");
                    Console.WriteLine($"л2 = {lambda2:F8}, f(л2) = {f2:F8}");
                }

                if (f1 > f2)
                {
                    if (useWrite) Console.WriteLine("f(л1) > f(л2), значит a = л1");
                    a = lambda1;
                    lambda1 = lambda2;
                    f1 = f2;
                    lambda2 = a + Phi2 * (b - a);
                    f2 = _function(lambda2);
                }
                else
                {
                    if (useWrite) Console.WriteLine("f(л1) <= f(л2), значит b = л2");
                    b = lambda2;
                    lambda2 = lambda1;
                    f2 = f1;
                    lambda1 = a + Phi1 * (b - a);
                    f1 = _function(lambda1);
                }
            }

            double x = (a + b) / 2;
            double fx = _function(x);

            if (useWrite)
            {
                Console.WriteLine("\nРезультаты вычислений:");
                Console.WriteLine($"Итераций выполнено: {iteration}");
                Console.WriteLine($"Интервал сжат до [{a:F8}, {b:F8}]");
                Console.WriteLine($"x = ({a:F8} + {b:F8}) / 2 = {x:F8}");
                Console.WriteLine($"f(x) = {fx:F8}");
            }

            return x;
        }
    }
}