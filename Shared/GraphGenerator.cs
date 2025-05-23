﻿using System.Drawing;
using System.Numerics;

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
                new(dataPoints, markerSize: 10, lineWidth: 0, color: Color.Black, label: "Ключевые точки")
            };

            return graphData;
        }

        public static List<GraphParameters> GenerateData(InterpolationBase<Complex> interpolator, Func<double, double> function, double xMin, double xStep, double xCount)
        {
            const double step = 0.01, margin = 0.1, smoothing = 100.0;

            List<(double x, double y)> dataPoints = interpolator.GetPoints();

            if (dataPoints == null || !dataPoints.Any())
            {
                return new List<GraphParameters>();
            }

            List<(double x, double y)> xyValuesRe = new();
            List<(double x, double y)> xyValuesIm = new();

            double minX = dataPoints.Min(p => p.x);
            double maxX = dataPoints.Max(p => p.x);

            const double viewStep = step / smoothing;
            double effectiveXMin = Math.Min(xMin, minX);
            double effectiveXMax = Math.Max(maxX, minX + (xCount - 1) * xStep);
            int viewCount = Convert.ToInt32(((effectiveXMax - effectiveXMin) / viewStep) + 1);
            List<(double x, double y)> viewDataPoints = ListUtils.FillDataPoints(function, effectiveXMin, viewStep, viewCount);

            double delta = (maxX - minX) * margin;
            if (Math.Abs(delta) < 1e-9) delta = 1.0;
            double plotMinX = minX - delta;
            double plotMaxX = maxX + delta;

            for (double x = plotMinX; x <= plotMaxX; x += step)
            {
                Complex complexY = interpolator.Compute(x, false);
                double yRe = complexY.Real;
                double yIm = complexY.Imaginary;

                if (double.IsFinite(yRe) && Math.Abs(yRe) < 1e6)
                {
                    xyValuesRe.Add((x, yRe));
                }
                if (double.IsFinite(yIm) && Math.Abs(yIm) < 1e6)
                {
                    xyValuesIm.Add((x, yIm));
                }
            }

            if (!xyValuesRe.Any() && !xyValuesIm.Any())
            {
                if (dataPoints.Any())
                {
                    xyValuesRe.AddRange(dataPoints);
                }
                else
                {
                    return new List<GraphParameters>();
                }
            }

            double yMinData = dataPoints.Any() ? dataPoints.Min(p => p.y) : double.MaxValue;
            double yMaxData = dataPoints.Any() ? dataPoints.Max(p => p.y) : double.MinValue;

            double yMinInterpRe = xyValuesRe.Any() ? xyValuesRe.Min(p => p.y) : double.MaxValue;
            double yMaxInterpRe = xyValuesRe.Any() ? xyValuesRe.Max(p => p.y) : double.MinValue;
            double yMinInterpIm = xyValuesIm.Any() ? xyValuesIm.Min(p => p.y) : double.MaxValue;
            double yMaxInterpIm = xyValuesIm.Any() ? xyValuesIm.Max(p => p.y) : double.MinValue;

            double overallYMin = Math.Min(yMinData, Math.Min(yMinInterpRe, yMinInterpIm));
            double overallYMax = Math.Max(yMaxData, Math.Max(yMaxInterpRe, yMaxInterpIm));

            if (overallYMin == double.MaxValue) overallYMin = -1.0;
            if (overallYMax == double.MinValue) overallYMax = 1.0;
            if (overallYMin >= overallYMax) overallYMax = overallYMin + 1.0;

            double yRange = overallYMax - overallYMin;
            double yPadding = yRange * 0.1;
            if (Math.Abs(yPadding) < 1e-9) yPadding = 1.0;
            double axisLimitMinY = overallYMin - yPadding;
            double axisLimitMaxY = overallYMax + yPadding;

            List<(double x, double y)> limitsLeft = new() { (minX, axisLimitMaxY), (minX, axisLimitMinY) };
            List<(double x, double y)> limitsRight = new() { (maxX, axisLimitMaxY), (maxX, axisLimitMinY) };

            List<GraphParameters> graphData = new()
            {
                new(viewDataPoints, markerSize: 0, lineWidth: 3, color: Color.Green, true, label: "Исходная функция"),
                new(dataPoints, markerSize: 10, lineWidth: 0, color: Color.Black, label: "Узлы интерполяции")
            };

            if (xyValuesRe.Any())
            {
                graphData.Insert(0, new(xyValuesRe, markerSize: 0, lineWidth: 5, color: Color.Blue, label: "Триг. интерполяция (Re)"));
            }

            if (xyValuesIm.Any())
            {
                graphData.Insert(1, new(xyValuesIm, markerSize: 0, lineWidth: 5, color: Color.Red, label: "Триг. интерполяция (Im)"));
            }

            return graphData;
        }

        public static List<GraphParameters> GenerateApproximationData(
            Func<double, double> originalFunction,
            Func<double, double> approximatedFunction,
            List<(double x, double y)> inputDataPoints,
            double xPlotMin,
            double xPlotMax,
            int numberOfPlotPoints = 200)
        {
            const double marginFactor = 0.1;

            List<(double x, double y)> originalFunctionPlotPoints = new();
            double plotStep = (xPlotMax - xPlotMin) / (numberOfPlotPoints - 1);

            for (int i = 0; i < numberOfPlotPoints; i++)
            {
                double x = xPlotMin + i * plotStep;
                double y = originalFunction(x);
                if (double.IsFinite(y) && Math.Abs(y) < 1e6)
                {
                    originalFunctionPlotPoints.Add((x, y));
                }
            }

            List<(double x, double y)> approximatedFunctionPlotPoints = new();
            for (int i = 0; i < numberOfPlotPoints; i++)
            {
                double x = xPlotMin + i * plotStep;
                double y = approximatedFunction(x);
                if (double.IsFinite(y) && Math.Abs(y) < 1e6)
                {
                    approximatedFunctionPlotPoints.Add((x, y));
                }
            }

            List<double> allXValues = new List<double>();
            allXValues.AddRange(inputDataPoints.Select(p => p.x));
            allXValues.AddRange(originalFunctionPlotPoints.Select(p => p.x));
            allXValues.AddRange(approximatedFunctionPlotPoints.Select(p => p.x));

            List<double> allYValues = new List<double>();
            allYValues.AddRange(inputDataPoints.Select(p => p.y));
            allYValues.AddRange(originalFunctionPlotPoints.Select(p => p.y));
            allYValues.AddRange(approximatedFunctionPlotPoints.Select(p => p.y));

            double minXGraph = allXValues.Any() ? allXValues.Min() : xPlotMin;
            double maxXGraph = allXValues.Any() ? allXValues.Max() : xPlotMax;
            double minYGraph = allYValues.Any() ? allYValues.Min() : -1;
            double maxYGraph = allYValues.Any() ? allYValues.Max() : 1;

            double xRange = maxXGraph - minXGraph;
            if (Math.Abs(xRange) < 1e-9) xRange = 1.0;
            minXGraph -= xRange * marginFactor;
            maxXGraph += xRange * marginFactor;

            double yRange = maxYGraph - minYGraph;
            if (Math.Abs(yRange) < 1e-9 && Math.Abs(minYGraph) < 1e-9)
            {
                minYGraph = -1.0;
                maxYGraph = 1.0;
            }
            else if (Math.Abs(yRange) < 1e-9)
            {
                minYGraph -= Math.Abs(minYGraph * marginFactor) + 0.5;
                maxYGraph += Math.Abs(maxYGraph * marginFactor) + 0.5;
            }
            else
            {
                minYGraph -= yRange * marginFactor;
                maxYGraph += yRange * marginFactor;
            }

            List<GraphParameters> graphData = new()
            {

                new(approximatedFunctionPlotPoints, markerSize: 0, lineWidth: 3, color: Color.Green, label: "Аппроксимирующая функция"),

                new(originalFunctionPlotPoints, markerSize: 0, lineWidth: 2, color: Color.Aqua, label: "Исходная функция"),

                new(inputDataPoints, markerSize: 15, lineWidth: 0, color: Color.Black, label: "Узлы аппроксимации")
            };

            return graphData;
        }
    }
}
