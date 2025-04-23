namespace _9
{
    abstract class IntegralCalculator
    {
        protected Func<double, double> _function;
        protected double _left;
        protected double _right;
        protected double _step;
        protected double _epsilon;

        public IntegralCalculator(Func<double, double> function, double left, double right, double step, double epsilon = 1e-6)
        {
            _function = function;
            _left = left;
            _right = right;
            _step = step;
            _epsilon = epsilon;
        }

        public abstract double Compute(bool useWrite);
    }
}
