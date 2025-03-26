using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Plotter;
using ScottPlot;

namespace InterpolationApp
{
    internal static class Program
    {
        private static readonly Dictionary<double, double> DataPoints = new()
        {
            { 1.0, 1.0000 },
            { 2.0, 1.4142 },
            { 3.0, 1.7321 },
            { 4.0, 2.0000 }
        };

        private const double LagrangeX = 2.56;
        private const double EitkenX = 1.75;
        private const double NewtonForwardX = 1.15;
        private const double NewtonBackwardX = 1.69;

        private static LagrangeInterpolator _lagrange;
        private static EitkenInterpolator _eitken;
        private static NewtonForwardInterpolator _newtonForward;
        private static NewtonBackwardInterpolator _newtonBackward;

        private static double _lagrangeResult;
        private static double _eitkenResult;
        private static double _newtonForwardResult;
        private static double _newtonBackwardResult;

        static void Main()
        {
            _lagrange = new LagrangeInterpolator(DataPoints);
            _eitken = new EitkenInterpolator(DataPoints);
            _newtonForward = new NewtonForwardInterpolator(DataPoints);
            _newtonBackward = new NewtonBackwardInterpolator(DataPoints);

            // Интерполяция методом Лагранжа.
            Console.WriteLine("Метод Лагранжа:\n");
            _lagrangeResult = _lagrange.Interpolate(LagrangeX);
            Console.WriteLine($"P({LagrangeX}) = {_lagrangeResult:F6}");

            Console.WriteLine("\nМетод Эйткена:\n");
            _eitkenResult = _eitken.Interpolate(EitkenX);
            Console.WriteLine($"P({EitkenX}) = {_eitkenResult:F6}");

            Console.WriteLine("\nМетод Ньютона (прямая формула):\n");
            Console.WriteLine("Таблица конечных разностей:");
            _newtonForward.PrintDifferenceTable();
            _newtonForwardResult = _newtonForward.Interpolate(NewtonForwardX);
            Console.WriteLine($"\nРезультат прямой формулы Ньютона: P({NewtonForwardX}) = {_newtonForwardResult:F6}");

            Console.WriteLine("\nМетод Ньютона (обратная формула):\n");
            _newtonBackwardResult = _newtonBackward.Interpolate(NewtonBackwardX);
            Console.WriteLine($"\nРезультат обратной формулы Ньютона: P({NewtonBackwardX}) = {_newtonBackwardResult:F6}");

            PlotInterpolationGraph();
        }

        /// <summary>
        /// Генерация и сохранение графика интерполяции.
        /// </summary>
        private static void PlotInterpolationGraph()
        {
            GraphPlotter plotter = new GraphPlotter(new Size(800, 600));
            plotter.SetTitle("Интерполяция: Лагранж, Эйткена, Ньютона");
            plotter.SetLabels("X", "Y");

            List<double> xOriginal = DataPoints.Keys.ToList();
            List<double> yOriginal = DataPoints.Values.ToList();
            plotter.AddData(xOriginal, yOriginal, "Исходные точки", scatter: true);

            List<double> xCurve = new();
            for (double x = 0; x <= 6.0; x += 0.2)
            {
                xCurve.Add(x);
            }
            List<double> yLagrangeCurve = xCurve.Select(x => _lagrange.Interpolate(x, debugOutput: false)).ToList();
            plotter.AddData(xCurve, yLagrangeCurve, "Лагранж", scatter: true);

            plotter.AddPoint(LagrangeX, _lagrangeResult, "P(LagrangeX)", size: 15);
            plotter.AddPoint(EitkenX, _eitkenResult, "P(EitkenX)", size: 15);
            plotter.AddPoint(NewtonForwardX, _newtonForwardResult, "P(NewtonForwardX)", size: 15);
            plotter.AddPoint(NewtonBackwardX, _newtonBackwardResult, "P(NewtonBackwardX)", size: 15);

            string outputPath = Path.Combine(AppContext.BaseDirectory, "interpolation_graph.png");
            plotter.Save(outputPath);
        }
    }
}
