using System;
using System.Collections.Generic;

namespace InterpolationApp
{
    /// <summary>
    /// Реализация интерполяции методом Эйткена.
    /// </summary>
    public class EitkenInterpolator
    {
        private readonly Dictionary<double, double> _dataPoints;

        public EitkenInterpolator(Dictionary<double, double> dataPoints)
        {
            _dataPoints = new Dictionary<double, double>(dataPoints);
        }

        public double Interpolate(double x, bool debugOutput = true)
        {
            int count = _dataPoints.Count;
            var xValues = new List<double>(_dataPoints.Keys);
            var yValues = new List<double>(_dataPoints.Values);
            double[,] p = new double[count, count];

            for (int i = 0; i < count; i++)
                p[i, 0] = yValues[i];

            for (int j = 1; j < count; j++)
            {
                for (int i = 0; i < count - j; i++)
                {
                    p[i, j] = ((x - xValues[i + j]) * p[i, j - 1] - (x - xValues[i]) * p[i + 1, j - 1])
                              / (xValues[i] - xValues[i + j]);

                    if (debugOutput)
                    {
                        Console.WriteLine(
                            $"P[{i},{j}] = (({x} - {xValues[i + j]}) * {p[i, j - 1]:F6} - ({x} - {xValues[i]}) * {p[i + 1, j - 1]:F6}) / ({xValues[i]} - {xValues[i + j]}) = {p[i, j]:F6}");
                    }
                }
            }

            return p[0, count - 1];
        }

        public void DisplayResult(double x)
        {
            double result = Interpolate(x);
            Console.WriteLine($"\nРезультат интерполяции Эйткена: P({x}) = {result:F6}");
        }
    }
}
