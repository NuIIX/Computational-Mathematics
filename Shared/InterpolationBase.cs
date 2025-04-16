namespace Shared
{
    public abstract class InterpolationBase<TResult>
    {
        protected List<(double x, double y)> _points;

        public int PointsCount => _points.Count;

        protected InterpolationBase(List<(double x, double y)> points)
        {
            if (points == null || points.Count < 2)
            {
                throw new ArgumentException("Необходимо минимум две точки для интерполяции");
            }

            _points = points.OrderBy(p => p.x).ToList();
        }

        public List<(double x, double y)> GetPoints() => _points;
        protected (double x, double y) GetPoint(int index) => _points[index];
        protected double GetX(int index) => _points[index].x;
        protected double GetY(int index) => _points[index].y;

        public abstract TResult Compute(double x, bool useWrite = true);
    }
}
