using System;

namespace InterpolationApp
{
    public class CubicSpline
    {
        public double[] x;
        public double[] y;
        public double[] M;
        public int n;

        public void InitializeData()
        {
            x = new double[] { 1, 3, 5, 7, 9 };
            y = new double[] { 2, 5, 2, -1, 2 };
            n = x.Length;
            Console.WriteLine("=== Кубическая интерполяция сплайнами ===");
            Console.WriteLine("Входные данные:");
            Console.WriteLine("  x: " + string.Join(", ", x));
            Console.WriteLine("  y: " + string.Join(", ", y));
            ComputeSecondDerivatives();
        }

        private void ComputeSecondDerivatives()
        {
            int nIntervals = n - 1;
            double[] h = new double[nIntervals];
            for (int i = 0; i < nIntervals; i++)
            {
                h[i] = x[i + 1] - x[i];
            }
            Console.WriteLine("\nВектор шагов h:");
            Console.WriteLine("  h: " + string.Join(", ", h));

            double[] A = new double[n - 2];
            double[] B = new double[n - 2];
            double[] C = new double[n - 2];
            double[] D = new double[n - 2];

            Console.WriteLine("\nСоставление тридиагональной системы:");
            for (int i = 1; i < n - 1; i++)
            {
                double hi = h[i - 1];
                double hi1 = h[i];
                A[i - 1] = hi;
                B[i - 1] = 2 * (hi + hi1);
                C[i - 1] = hi1;
                double di = (y[i + 1] - y[i]) / hi1 - (y[i] - y[i - 1]) / hi;
                D[i - 1] = 6 * di;
                Console.WriteLine($"  Для i={i}: A={A[i - 1]:F4}, B={B[i - 1]:F4}, C={C[i - 1]:F4}, D={D[i - 1]:F4}");
            }
            
            double[] M_internal = SolveTridiagonal(A, B, C, D);
            Console.WriteLine("\nРешение тридиагональной системы (внутренние M):");
            for (int i = 0; i < M_internal.Length; i++)
            {
                Console.WriteLine($"  M{i + 1} = {M_internal[i]:F4}");
            }

            M = new double[n];
            M[0] = 0;
            M[n - 1] = 0;
            for (int i = 1; i < n - 1; i++)
            {
                M[i] = M_internal[i - 1];
            }
            Console.WriteLine("\nПолный вектор вторых производных M:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"  M[{i}] = {M[i]:F4}");
            }
        }

        private double[] SolveTridiagonal(double[] a, double[] b, double[] c, double[] d)
        {
            int n = b.Length;
            double[] cp = new double[n];
            double[] dp = new double[n];
            double[] xSol = new double[n];

            cp[0] = c[0] / b[0];
            dp[0] = d[0] / b[0];
            for (int i = 1; i < n; i++)
            {
                double m = b[i] - a[i] * cp[i - 1];
                cp[i] = (i < n - 1) ? c[i] / m : 0;
                dp[i] = (d[i] - a[i] * dp[i - 1]) / m;
            }
            xSol[n - 1] = dp[n - 1];
            for (int i = n - 2; i >= 0; i--)
            {
                xSol[i] = dp[i] - cp[i] * xSol[i + 1];
            }
            return xSol;
        }

        public double Evaluate(double X)
        {
            int i;
            for (i = 1; i < n; i++)
            {
                if (X <= x[i])
                    break;
            }
            if (i == n) i = n - 1;
            int idx = i - 1;
            double hi = x[i] - x[i - 1];
            double A = (x[i] - X) / hi;
            double B = (X - x[i - 1]) / hi;
            double splineValue = A * y[i - 1] + B * y[i] + ((A * A * A - A) * M[i - 1] + (B * B * B - B) * M[i]) * (hi * hi) / 6.0;
            Console.WriteLine($"\nВычисление S({X:F2}) в интервале [{x[i - 1]}, {x[i]}]:");
            Console.WriteLine($"  hi = {hi:F4}, A = {A:F4}, B = {B:F4}");
            Console.WriteLine($"  A*y[{i - 1}] = {A * y[i - 1]:F4}, B*y[{i}] = {B * y[i]:F4}");
            Console.WriteLine($"  Дополнительный член = {((A * A * A - A) * M[i - 1] + (B * B * B - B) * M[i]) * (hi * hi) / 6.0:F4}");
            Console.WriteLine($"  S({X:F2}) = {splineValue:F4}");
            return splineValue;
        }
    }
}
