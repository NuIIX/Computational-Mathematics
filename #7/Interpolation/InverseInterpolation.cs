using System;

namespace InterpolationApp
{
    public class InverseInterpolation
    {
        public double[] x;
        public double[] y;
        public int n;

        public void InitializeData()
        {
            // Данные, приведённые в ДЗ: точки (x, y) для обратной интерполяции:
            // x: {3, -2, -1} соответствуют y: {6, -1, -6}
            x = new double[] { 3, -2, -1 };
            y = new double[] { 6, -1, -6 };
            n = x.Length;
            Console.WriteLine("\n=== Обратная интерполяция (формула Лагранжа) ===");
            Console.WriteLine("Входные данные:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"  Точка {i}: x = {x[i]}, y = {y[i]}");
            }
        }

        public double Evaluate(double Y)
        {
            Console.WriteLine($"\nВычисление обратной интерполяции для Y = {Y:F4}");
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                double term = x[i];
                string termDetails = $"  Базис L[{i}]({Y:F4}) = ";
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = (Y - y[j]) / (y[i] - y[j]);
                        term *= factor;
                        termDetails += $"((Y - ({y[j]})) / ({y[i]} - {y[j]})) ";
                    }
                }
                Console.WriteLine(termDetails + $"=> вклад: {term:F4}");
                result += term;
            }
            Console.WriteLine($"Итоговый результат обратной интерполяции P({Y:F4}) = {result:F4}");
            return result;
        }
    }
}
