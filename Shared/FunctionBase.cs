namespace Shared
{
    public abstract class FunctionBase : PrecisionSettings
    {
        protected readonly Func<double, double> _function;
        protected readonly Func<double, double> _derivative;

        public FunctionBase(Func<double, double> function, Func<double, double>? derivative = null, double tolerance = 1e-6)
            : base(tolerance)
        {
            _function = function;
            _derivative = derivative ?? (_ => double.NaN);
        }
    }
}
