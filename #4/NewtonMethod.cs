namespace _4
{
    public class NewtonMethod : FunctionBase
    {
        public NewtonMethod(Func<double, double> function, Func<double, double> derivative, double tolerance = 1e-6)
            : base(function, derivative, tolerance) { }

        public double Solve(double x0)
        {
            Console.WriteLine($"Начальная точка: {Round(x0)}");

            double x = x0, xNew, epsilon = double.MaxValue;
            int step = 1;

            while (epsilon >= _tolerance)
            {
                double fx = _function(x);
                double dfx = _derivative(x);

                if (Math.Abs(dfx) < _tolerance)
                    throw new Exception("Производная слишком мала, метод Ньютона не применим.");

                xNew = x - fx / dfx;
                epsilon = Math.Abs(x - xNew);

                PrintStep(step, x, xNew, epsilon);

                if (epsilon < _tolerance)
                    return xNew;

                x = xNew;
                step++;
            }

            return x;
        }

        private void PrintStep(int step, double xPrev, double xNew, double epsilon)
        {
            Console.WriteLine($"\nШаг {step}:");
            Console.WriteLine($"x{step} = {Round(xNew)}");
            Console.WriteLine($"f(x{step}) = {Round(_function(xNew))}");
            Console.WriteLine($"E = |x{step} - x{step - 1}| = {Round(epsilon)}");
        }
    }
}
