using System.Drawing;

namespace Shared
{
    public static class GraphGenerator
    {
        public static List<GraphParameters> GenerateData(InterpolationBase<double> interpolator, Func<double, double> function, double xMin, double xStep, double xCount)
        {
            const double step = 0.01, margin = 1, smoothing = 100.0;

            List<(double x, double y)> dataPoints = interpolator.GetPoints(), xyValues = new();

            const double viewStep = step / smoothing;
            double xMax = dataPoints.Max().x;
            int viewCount = Convert.ToInt32(((xMax - xMin) / viewStep) + 1);

            List<(double x, double y)> viewDataPoints = ListUtils.FillDataPoints(function, xMin, viewStep, viewCount);

            double minX = dataPoints.Min(p => p.x);
            double maxX = dataPoints.Max(p => p.x);

            double delta = (maxX - minX) * margin;
            double plotMinX = minX - delta;
            double plotMaxX = maxX + delta;

            for (double x = plotMinX; x <= plotMaxX; x += step)
            {
                double y = interpolator.Compute(x, false);

                if (double.IsFinite(y) && Math.Abs(y) < 1e6)
                {
                    xyValues.Add((x, y));
                }
            }

            double yMin = xyValues.Min(p => p.y);
            double yMax = xyValues.Max(p => p.y);
            double yRange = yMax - yMin;
            double axisLimit = Math.Max(Math.Abs(yMax), Math.Abs(yMin)) * 1.2;

            List<(double x, double y)> limitsLeft = new()
            {
                (minX, axisLimit),
                (minX, -axisLimit)
            };

            List<(double x, double y)> limitsRight = new()
            {
                (maxX, axisLimit),
                (maxX, -axisLimit)
            };

            List<GraphParameters> graphData = new()
            {
                new(xyValues, markerSize: 0, lineWidth: 6, color: Color.Green, label: "Линия интерполяции"),
                new(viewDataPoints, markerSize: 0, lineWidth: 2, color: Color.Aqua, true, label: "Исходные данные"),
                new(dataPoints, markerSize: 10, lineWidth: 0, color: Color.Black, label: "Ключевые точки"),
                new(limitsLeft, 0, 3, Color.Gray),
                new(limitsRight, 0, 3, Color.Gray)
            };

            return graphData;
        }
    }
}
