using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    public abstract class InterpolationBase
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

        protected (double x, double y) GetPoint(int index) => _points[index];

        protected double GetX(int index) => _points[index].x;

        protected double GetY(int index) => _points[index].y;
    }
}
