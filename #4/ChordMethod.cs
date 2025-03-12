namespace _4
{
    public class ChordMethod : RootFindingMethod
    {
        public ChordMethod(Func<double, double> function, double tolerance = 1e-6)
            : base(function, tolerance) { }

        protected override double FindC(double a, double b)
        {
            return (a * _function(b) - b * _function(a)) / (_function(b) - _function(a));
        }

        public override double Solve(double a, double b)
        {
            if (_function(a) * _function(b) >= 0)
            {
                throw new ArgumentException("Функция должна иметь разные знаки на концах интервала.");
            }

            Console.WriteLine($"Начальный интервал: [{Round(a)}; {Round(b)}]");

            int step = 1;
            double c, epsilon;

            while ((b - a) / 2 > _tolerance)
            {
                c = FindC(a, b);
                epsilon = (b - a) / 2;

                PrintStep(step, a, b, c, epsilon);

                if (Math.Abs(_function(c)) < _tolerance)
                {
                    return c;
                }

                if (_function(a) * _function(c) < 0)
                {
                    b = c;
                }
                else
                {
                    a = c;
                }

                step++;
            }

            return (a + b) / 2;
        }
    }
}
