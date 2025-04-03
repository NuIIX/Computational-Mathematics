using System;
using System.Numerics;

namespace InterpolationApp
{
    public class TrigonometricInterpolation
    {
        // Тригонометрическая интерполяция для равномерной сетки.
        // Дано: x: {0, 1, 2, 3}, y: {0, 1, 4, 9} (n=4, период T = 4, шаг h = 1)
        public double[] x;
        public double[] y;
        public int n;
        public Complex[] A; // коэффициенты Фурье

        public void InitializeData()
        {
            x = new double[] { 0, 1, 2, 3 };
            y = new double[] { 0, 1, 4, 9 };
            n = x.Length;
            Console.WriteLine("\n=== Тригонометрическая интерполяция ===");
            Console.WriteLine("Входные данные:");
            Console.WriteLine("  x: " + string.Join(", ", x));
            Console.WriteLine("  y: " + string.Join(", ", y));
            ComputeFourierCoefficients();
        }

        private void ComputeFourierCoefficients()
        {
            A = new Complex[n];
            Console.WriteLine("\nВычисление коэффициентов Фурье (ДПФ):");
            // Используем формулу ДПФ: A[k] = (1/n)*Σ y[j]*exp(-2πikj/n)
            for (int k = 0; k < n; k++)
            {
                Complex sum = Complex.Zero;
                for (int j = 0; j < n; j++)
                {
                    double angle = -2 * Math.PI * k * j / n;
                    sum += y[j] * Complex.Exp(new Complex(0, angle));
                }
                A[k] = sum / n;
                Console.WriteLine($"  A[{k}] = {A[k].Real:F4} + {A[k].Imaginary:F4}i");
            }
        }

        public double Interpolate(double X)
        {
            Console.WriteLine($"\nВычисление тригонометрической интерполяции для X = {X:F4}");
            // Тригонометрическая интерполяция: P(X)=Σ A[k]*exp(2πikX/n)
            Complex result = Complex.Zero;
            for (int k = 0; k < n; k++)
            {
                double angle = 2 * Math.PI * k * X / n;
                Complex term = A[k] * Complex.Exp(new Complex(0, angle));
                Console.WriteLine($"  Для k={k}: exp(2πi*{k}*{X:F4}/{n}) = {Complex.Exp(new Complex(0, angle)).Real:F4}+{Complex.Exp(new Complex(0, angle)).Imaginary:F4}i, вклад = {term.Real:F4}+{term.Imaginary:F4}i");
                result += term;
            }
            Console.WriteLine($"Итоговая интерполяция y({X:F4}) = {result.Real:F4} (игнорируя мнимую часть)");
            return result.Real;
        }
    }
}
