namespace _2
{
    internal class NumberWithError
    {
        public double Value { get; }
        public double Error { get; }

        public NumberWithError(double value, double error)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Значение не должно быть NaN или Inf!");
            }

            if (error < 0)
            {
                throw new ArgumentException("Погрешность должна быть больше нуля!");
            }

            Value = value;
            Error = error;
        }

        public static NumberWithError operator +(NumberWithError a, NumberWithError b)
        {
            return new NumberWithError(a.Value + b.Value, a.Error + b.Error);
        }

        public static NumberWithError operator -(NumberWithError a, NumberWithError b)
        {
            return new NumberWithError(a.Value - b.Value, a.Error + b.Error);
        }

        public static NumberWithError operator *(NumberWithError a, NumberWithError b)
        {
            if (a.Value == 0 || b.Value == 0)
            {
                return new NumberWithError(0, 0);
            }

            double relativeError = (a.Error / a.Value) + (b.Error / b.Value);
            double newValue = a.Value * b.Value;
            double newError = newValue * relativeError;

            return new NumberWithError(newValue, newError);
        }

        public static NumberWithError operator /(NumberWithError a, NumberWithError b)
        {
            if (b.Value == 0) {
                throw new DivideByZeroException("Деление на ноль!");
            }

            if (a.Value == 0) {
                return new NumberWithError(0, 0);
            }

            double relativeError = (a.Error / a.Value) + (b.Error / b.Value);
            double newValue = a.Value / b.Value;
            double newError = Math.Abs(newValue) * relativeError;

            return new NumberWithError(newValue, newError);
        }

        public NumberWithError Pow(double power)
        {
            if (Value < 0 && power % 1 != 0) {
                throw new ArgumentException("Нельзя возвести отрицательное число в нецелочисленную степень!");
            }

            double newValue = Math.Pow(Value, power);
            double relativeError = Math.Abs(power) * (Error / Value);
            double newError = Math.Abs(newValue) * relativeError;

            return new NumberWithError(newValue, newError);
        }

        public NumberWithError Pow(NumberWithError power)
        {
            if (Value <= 0) {
                throw new ArgumentException("Значение должно быть больше нуля!");
            }

            NumberWithError exponentPower = Ln() * power;
            double exponentValue = Math.Exp(exponentPower.Value);
            double exponentError = Math.Abs(exponentValue) * exponentPower.Error;

            return new NumberWithError(exponentValue, exponentError);
        }

        public NumberWithError Sqrt()
        {
            if (Value < 0) {
                throw new ArgumentException("Нельзя вычислить корень из отрицательного числа!");
            }

            return Pow(0.5);
        }

        public NumberWithError Sin()
        {
            double newValue = Math.Sin(Value);
            double newError = Math.Abs(Math.Cos(Value) * Error);

            return new NumberWithError(newValue, newError);
        }

        public NumberWithError Cos()
        {
            double newValue = Math.Cos(Value);
            double newError = Math.Abs(Math.Sin(Value) * Error);

            return new NumberWithError(newValue, newError);
        }

        public NumberWithError Ln()
        {
            if (Value <= 0) {
                throw new ArgumentException("Нельзя вычислить логарифм из отрицательного числа!");
            }

            double newValue = Math.Log(Value);
            double newError = Math.Abs(Error / Value);

            return new NumberWithError(newValue, newError);
        }

        public override string ToString()
        {
            return $"{Value:F4} ± {Error:F4}";
        }
    }
}
