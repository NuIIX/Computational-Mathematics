using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpolationApp
{
    /// <summary>
    /// Базовый класс для реализации интерполяции по методу Ньютона.
    /// Содержит общий функционал для вычисления таблицы конечных разностей.
    /// </summary>
    public abstract class NewtonInterpolator
    {
        protected readonly Dictionary<double, double> Points;
        protected readonly double[,] DifferenceTable;

        protected NewtonInterpolator(Dictionary<double, double> dataPoints)
        {
            Points = new Dictionary<double, double>(dataPoints);
            DifferenceTable = BuildDifferenceTable();
        }

        /// <summary>
        /// Вычисление таблицы конечных разностей.
        /// </summary>
        private double[,] BuildDifferenceTable()
        {
            int count = Points.Count;
            var table = new double[count, count];
            var yValues = Points.Values.ToList();

            for (int i = 0; i < count; i++)
                table[i, 0] = yValues[i];

            for (int j = 1; j < count; j++)
            {
                for (int i = 0; i < count - j; i++)
                {
                    table[i, j] = table[i + 1, j - 1] - table[i, j - 1];
                }
            }

            return table;
        }

        protected static double Factorial(int n)
        {
            double result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        /// <summary>
        /// Печать таблицы конечных разностей.
        /// </summary>
        public void PrintDifferenceTable()
        {
            int count = Points.Count;
            var xValues = Points.Keys.ToList();

            Console.WriteLine("x\t\ty\t\tdy\t\td2y\t\td3y");
            for (int i = 0; i < count; i++)
            {
                Console.Write($"{xValues[i]}\t");
                for (int j = 0; j < count - i; j++)
                {
                    Console.Write($"{DifferenceTable[i, j]:F6}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public abstract double Interpolate(double x, bool debugOutput = true);
    }
}
