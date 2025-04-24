using PlotterForms;
using Shared;

namespace _8
{
    internal static class Program
    {
        static Func<double, double> function = Math.Sqrt;

        const double xMin = 0.0, xStep = 1.0;
        const int xCount = 4;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, xStep, xCount);

        const double xTarget = 1.5;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TrigonometricInterpolation trigInterpolator = new(dataPoints);

            RunConsoleCalculation();

            List<GraphParameters> graphData = GraphGenerator.GenerateData(trigInterpolator, function, xMin, xStep, xCount);

            Graph graphForm = new Graph();
            graphForm.Text = "График Тригонометрической Интерполяции";

            if (graphData.Any())
            {
                foreach (var param in graphData)
                {
                    graphForm.PlotPoints(param);
                }

                graphForm.ShowLegendTable();
            }
            else
            {
                MessageBox.Show("Не удалось сгенерировать данные для графика.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Application.Run(graphForm);
        }

        static void RunConsoleCalculation()
        {
            Console.WriteLine("Расчет для тригонометрической интерполяции:");
            TrigonometricInterpolation trigInterpolator = new(dataPoints);
            Printer.PrintListAsTable(dataPoints, "Исходные данные (узлы)");
            trigInterpolator.Compute(xTarget, useWrite: true);
            Console.WriteLine("\n" + new string('-', 30) + "\n");
        }
    }
}
