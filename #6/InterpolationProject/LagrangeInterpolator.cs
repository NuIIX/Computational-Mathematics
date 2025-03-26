using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpolationApp
{
    /// <summary>
    /// Реализация интерполяции методом Лагранжа.
    /// </summary>
    public class LagrangeInterpolator
    {
        private readonly Dictionary<double, double> _dataPoints;

        public LagrangeInterpolator(Dictionary<double, double> dataPoints)
        {
            _dataPoints = new Dictionary<double, double>(dataPoints);
        }

        public double Interpolate(double x, bool debugOutput = true)
        {
            double result = 0;
            var xValues = _dataPoints.Keys.ToList();
            var yValues = _dataPoints.Values.ToList();
            int count = xValues.Count;

            for (int i = 0; i < count; i++)
            {
                double term = yValues[i];

                for (int j = 0; j < count; j++)
                {
                    if (i != j)
                    {
                        term *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                        if (debugOutput)
                        {
                            Console.Write($"(x - {xValues[j]})/({xValues[i]} - {xValues[j]}) * ");
                        }
                    }
                }

                result += term;
                if (debugOutput)
                {
                    Console.WriteLine($"{yValues[i]} -> {term}");
                }
            }

            return result;
        }

        /// <summary>
        /// Вычисление погрешности усечения.
        /// </summary>
        public double ComputeTruncationError(double x)
        {
            double maxDerivative = 15.0 / 16;
            double factorial = 24.0;
            double product = 1;

            foreach (var xi in _dataPoints.Keys)
            {
                product *= (x - xi);
            }

            return Math.Abs((maxDerivative / factorial) * product);
        }

        public void DisplayResult(double x)
        {
            double interpolatedValue = Interpolate(x);
            double truncationError = ComputeTruncationError(x);
            double roundingError = 5e-5;
            double totalError = truncationError + roundingError;

            Console.WriteLine();
            Console.WriteLine($"Значение в x = {x}: {interpolatedValue:F4}");
            Console.WriteLine($"Погрешность усечения: {truncationError:F6}");
            Console.WriteLine($"Погрешность округления: {roundingError:F6}");
            Console.WriteLine($"Реальная погрешность: {totalError:F6}");
        }
    }
}
