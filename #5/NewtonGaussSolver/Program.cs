using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NewtonGaussSolver
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Решение системы нелинейных уравнений методом Ньютона через Гаусса");
            Console.WriteLine("Система:");
            Console.WriteLine("    F1(x, y) = x^2 + y^2 - 4 = 0");
            Console.WriteLine("    F2(x, y) = x/y - 2 = 0");
            Console.WriteLine("---------------------------------------------------");

            double tolerance = 1e-14;
            int maxIterations = 100;
            double[] x = { 2.0, 1.0 };
            List<double[]> iterationPoints = new List<double[]> { new double[] { x[0], x[1] } };

            for (int iter = 0; iter < maxIterations; iter++)
            {
                double[] F = ComputeF(x[0], x[1]);
                Console.WriteLine($"Итерация {iter+1}:");
                Console.WriteLine($"  Текущее приближение: x = {x[0]:F6}, y = {x[1]:F6}");
                Console.WriteLine($"  F(x) = ( {F[0]:F6}, {F[1]:F6} )");

                double[,] J = ComputeJacobian(x[0], x[1]);
                Console.WriteLine("  Якобиан W(x):");
                Console.WriteLine($"      [{J[0, 0]:F6}\t{J[0, 1]:F6}]");
                Console.WriteLine($"      [{J[1, 0]:F6}\t{J[1, 1]:F6}]");

                double[] Y = SolveLinearSystem(J, F);
                Console.WriteLine($"  Коррекция Y = ( {Y[0]:F6}, {Y[1]:F6} )");

                double newX = x[0] - Y[0];
                double newY = x[1] - Y[1];
                Console.WriteLine($"  Новое приближение: x = {newX:F6}, y = {newY:F6}");
                Console.WriteLine("---------------------------------------------------");

                iterationPoints.Add(new double[] { newX, newY });

                if (Math.Sqrt(Y[0] * Y[0] + Y[1] * Y[1]) < tolerance)
                {
                    Console.WriteLine("Достигнута требуемая точность!");
                    x[0] = newX;
                    x[1] = newY;
                    break;
                }
                x[0] = newX;
                x[1] = newY;
            }

            Console.WriteLine("Итерационные точки:");
            foreach (var pt in iterationPoints)
            {
                Console.WriteLine($"  ({pt[0]:F6}, {pt[1]:F6})");
            }

            ShowChart(iterationPoints);

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static double[] ComputeF(double x, double y)
        {
            double f1 = x * x + Math.Pow(y, 3) - 4;
            double f2 = x / y - 2;
            return new double[] { f1, f2 };
        }

        static double[,] ComputeJacobian(double x, double y)
        {
            return new double[,]
            {
                { 2 * x, 3 * y * y },
                { 1.0 / y, -x / (y * y) }
            };
        }

        static double[] SolveLinearSystem(double[,] A, double[] b)
        {
            double a11 = A[0, 0], a12 = A[0, 1],
                   a21 = A[1, 0], a22 = A[1, 1];
            double b1 = b[0], b2 = b[1];

            double factor = a21 / a11;
            a22 -= factor * a12;
            b2 -= factor * b1;

            double Y2 = b2 / a22;
            double Y1 = (b1 - a12 * Y2) / a11;

            return new double[] { Y1, Y2 };
        }

        static void ShowChart(List<double[]> points)
        {
            Chart chart = new Chart { Size = new Size(600, 400) };
            ChartArea chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);
            Series series = new Series("Итерации")
            {
                ChartType = SeriesChartType.Line,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8
            };

            foreach (var pt in points)
            {
                series.Points.AddXY(pt[0], pt[1]);
            }

            chart.Series.Add(series);
            chart.Titles.Add("Итерационные точки метода Ньютона");
            chart.ChartAreas[0].AxisX.Title = "x";
            chart.ChartAreas[0].AxisY.Title = "y";

            Form form = new Form { Text = "График итераций", ClientSize = new Size(620, 420) };
            chart.Dock = DockStyle.Fill;
            form.Controls.Add(chart);
            Application.Run(form);
        }
    }
}
