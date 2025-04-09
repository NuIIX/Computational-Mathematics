using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class ListUtils
    {
        public static List<(double, double)> FillDataPoints(Func<double, double> function, double xMin, double step, int xCount) =>
            Enumerable
            .Range(0, xCount)
            .Select(i => (xMin + i * step, function(xMin + i * step)))
            .ToList();
    }
}
