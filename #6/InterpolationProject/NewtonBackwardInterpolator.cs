using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpolationApp
{
    /// <summary>
    /// Интерполяция по формуле Ньютона (обратная разностная формула).
    /// </summary>
    public class NewtonBackwardInterpolator : NewtonInterpolator
    {
        public NewtonBackwardInterpolator(Dictionary<double, double> dataPoints)
            : base(dataPoints) { }

        public override double Interpolate(double x, bool debugOutput = true)
        {
            var xValues = Points.Keys.ToList();
            int count = xValues.Count;
            double h = xValues[1] - xValues[0];
            double q = (x - xValues[count - 1]) / h;

            if (debugOutput)
                Console.WriteLine($"q = ({x} - {xValues[count - 1]})/{h} = {q:F6}");

            double result = DifferenceTable[count - 1, 0];
            double term = 1;

            for (int i = 1; i < count; i++)
            {
                term *= (q + (i - 1)) / i;
                double addTerm = DifferenceTable[count - i - 1, i] * term;
                result += addTerm;

                if (debugOutput)
                    Console.WriteLine($"Шаг {i}: {DifferenceTable[count - i - 1, i]:F6} / {Factorial(i)} * {term:F6} = {addTerm:F6}");
            }

            return result;
        }
    }
}
