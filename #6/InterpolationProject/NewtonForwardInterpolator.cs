using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpolationApp
{
    /// <summary>
    /// Интерполяция по формуле Ньютона (прямая разностная формула).
    /// </summary>
    public class NewtonForwardInterpolator : NewtonInterpolator
    {
        public NewtonForwardInterpolator(Dictionary<double, double> dataPoints)
            : base(dataPoints) { }

        public override double Interpolate(double x, bool debugOutput = true)
        {
            var xValues = Points.Keys.ToList();
            double h = xValues[1] - xValues[0];
            double q = (x - xValues[0]) / h;

            if (debugOutput)
                Console.WriteLine($"q = ({x} - {xValues[0]})/{h} = {q:F6}");

            double result = DifferenceTable[0, 0];
            double term = 1;

            for (int i = 1; i < Points.Count; i++)
            {
                term *= (q - (i - 1)) / i;
                double addTerm = DifferenceTable[0, i] * term;
                result += addTerm;

                if (debugOutput)
                    Console.WriteLine($"Шаг {i}: {DifferenceTable[0, i]:F6} / {Factorial(i)} * {term:F6} = {addTerm:F6}");
            }

            return result;
        }
    }
}
