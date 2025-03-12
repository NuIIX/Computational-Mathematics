namespace _4
{
    public abstract class FunctionBase
    {
        protected readonly Func<double, double> _function;
        protected readonly Func<double, double> _derivative;
        protected readonly double _tolerance;
        private readonly int _precision;

        public FunctionBase(Func<double, double> function, Func<double, double> derivative = null, double tolerance = 1e-6)
        {
            _function = function;
            _derivative = derivative;
            _tolerance = tolerance;
            _precision = Math.Max(1, (int)Math.Ceiling(-Math.Log10(tolerance)) + 1); 
        }

        protected string Round(double value) => value.ToString($"F{_precision}");
    }
}
