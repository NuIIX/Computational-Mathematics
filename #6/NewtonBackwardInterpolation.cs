namespace _6
{
    public class NewtonBackwardInterpolation : NewtonInterpolation<double>
    {
        public NewtonBackwardInterpolation(List<(double x, double y)> dataPoints) : base(dataPoints) { }

        public override double Compute(double x, bool useWrite = true)
        {
            double h = _points[1].x - _points[0].x;
            double q = (x - _points[PointsCount - 1].x) / h;

            if (useWrite) Console.WriteLine($"q = ({x} - {_points[PointsCount - 1].x}) / {h} = {q:F6}");

            double result = _differenceTable[PointsCount - 1, 0];
            double term = 1;

            for (int i = 1; i < PointsCount; i++)
            {
                term *= (q + (i - 1)) / i;
                double addTerm = _differenceTable[PointsCount - i - 1, i] * term;
                result += addTerm;

                if (useWrite) Console.WriteLine($"Шаг {i}: {_differenceTable[PointsCount - i - 1, i]:F6} / {Factorial(i)} * {term:F6} = {addTerm:F6}");
            }

            return result;
        }
    }
}
