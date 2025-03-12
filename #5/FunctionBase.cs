using System;

namespace NewtonSolver
{
    public abstract class FunctionBase
    {
        protected readonly double Tolerance;
        private readonly int _precision;

        protected FunctionBase(double tolerance = 1e-6)
        {
            Tolerance = tolerance;
            _precision = Math.Max(1, (int)Math.Ceiling(-Math.Log10(tolerance)) + 1);
        }

        protected string Format(double value) => value.ToString($"F{_precision}");
    }
}
