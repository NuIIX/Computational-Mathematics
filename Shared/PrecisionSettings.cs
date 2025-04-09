namespace Shared
{
    public abstract class PrecisionSettings
    {
        protected readonly double _tolerance;
        private readonly int _precision;

        protected PrecisionSettings(double tolerance = 1e-6)
        {
            _tolerance = tolerance;
            _precision = Math.Max(1, (int)Math.Ceiling(-Math.Log10(tolerance)) + 1);
        }

        protected string Round(double value) => value.ToString($"F{_precision}");
    }
}
