namespace _10
{
    public abstract class Approximation
    {
        protected List<(double x, double y)> points;

        protected Approximation(List<(double x, double y)> dataPoints)
        {
            points = dataPoints;
        }

        public abstract Func<double, double> Compute(bool useWrite = true);
    }
}
