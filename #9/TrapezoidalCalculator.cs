namespace _9
{
    class TrapezoidalCalculator : IntegralCalculator
    {
        public TrapezoidalCalculator(Func<double, double> function, double left, double right, double step, double epsilon = 1e-8)
            : base(function, left, right, step, epsilon) { }

        public override double Compute(bool useWrite = true)
        {
            double previousResult = 0;
            double currentStep = _step;
            int iteration = 0;

            while (true)
            {
                double result = 0;
                int n = (int)((_right - _left) / currentStep);
                double h = (_right - _left) / n;

                iteration++;
                if (useWrite) Console.WriteLine($"\nИтерация {iteration}: шаг = {h}");

                for (int i = 1; i < n; i++)
                {
                    result += _function(_left + i * h);
                }

                result = h / 2 * (_function(_left) + 2 * result + _function(_right));
                
                if (useWrite) Console.WriteLine($"Результат: {result}");

                if (Math.Abs(result - previousResult) < _epsilon)
                {
                    if (useWrite) Console.WriteLine("Достигнута заданная точность.");
                    
                    return result;
                }
                else
                {
                    if (useWrite) Console.WriteLine("Точность не достигнута, продолжаем пересчет...");

                    previousResult = result;
                    currentStep /= 2;
                }
            }
        }
    }
}
