namespace _9
{
    class SimpsonCalculator : IntegralCalculator
    {
        public SimpsonCalculator(Func<double, double> function, double left, double right, double step, double epsilon = 1e-6)
            : base(function, left, right, step, epsilon) { }

        public override double Compute(bool useWrite = true)
        {
            double previousResult = 0;
            double currentStep = _step;
            int iteration = 0;

            while (true)
            {
                int n = (int)((_right - _left) / currentStep);
                
                if (n % 2 != 0) n++;

                double h = (_right - _left) / n;
                double result = _function(_left) + _function(_right);

                iteration++;

                if (useWrite) Console.WriteLine($"\nИтерация {iteration}: шаг = {h}");

                for (int i = 1; i < n; i++)
                {
                    double x = _left + i * h;
                    result += (i % 2 == 0 ? 2 : 4) * _function(x);
                }

                result *= h / 3;

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
