using System;

namespace InterpolationApp
{
    public class MultiDimensionalInterpolation
    {
        // Двухмерная интерполяция по сетке.
        // Дано: x: {0.8, 1.1, 1.4}, y: {0.9, 1.1} и f(x,y)=1/(x+y)
        public double[] xValues;
        public double[] yValues;
        public double[,] fValues; // fValues[i,j] = f(x_j, y_i)
        public int nx, ny;

        public void InitializeData()
        {
            xValues = new double[] { 0.8, 1.1, 1.4 };
            yValues = new double[] { 0.9, 1.1 };
            nx = xValues.Length;
            ny = yValues.Length;
            fValues = new double[ny, nx];
            Console.WriteLine("\n=== Двухмерная интерполяция ===");
            Console.WriteLine("Входные данные:");
            Console.WriteLine("  x: " + string.Join(", ", xValues));
            Console.WriteLine("  y: " + string.Join(", ", yValues));
            Console.WriteLine("  f(x,y)=1/(x+y):");
            for (int i = 0; i < ny; i++)
            {
                for (int j = 0; j < nx; j++)
                {
                    fValues[i, j] = 1.0 / (xValues[j] + yValues[i]);
                    Console.WriteLine($"    f({xValues[j]}, {yValues[i]}) = {fValues[i, j]:F4}");
                }
            }
        }

        // Двухступенчатая (Лагранжева) интерполяция:
        // Сначала интерполируем по x для каждого фиксированного y, затем по y.
        public double Interpolate(double X, double Y)
        {
            Console.WriteLine($"\nВычисление многомерной интерполяции для (X,Y)=({X:F4}, {Y:F4})");
            double[] interpX = new double[ny];
            for (int i = 0; i < ny; i++)
            {
                double[] row = GetRow(i);
                interpX[i] = LagrangeInterpolation(xValues, row, X);
                Console.WriteLine($"  Интерполированное значение по x для y={yValues[i]:F4} -> P_x = {interpX[i]:F4}");
            }
            double result = LagrangeInterpolation(yValues, interpX, Y);
            Console.WriteLine($"Итоговое интерполированное значение f({X:F4},{Y:F4}) = {result:F4}");
            return result;
        }

        private double[] GetRow(int rowIndex)
        {
            double[] row = new double[nx];
            for (int j = 0; j < nx; j++)
            {
                row[j] = fValues[rowIndex, j];
            }
            return row;
        }

        private double LagrangeInterpolation(double[] xs, double[] ys, double X)
        {
            double result = 0;
            int n = xs.Length;
            for (int i = 0; i < n; i++)
            {
                double term = ys[i];
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                        term *= (X - xs[j]) / (xs[i] - xs[j]);
                }
                result += term;
            }
            return result;
        }
    }
}
