using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InterpolationApp
{
    public class ChartForm : Form
    {
        private TabControl tabControl;
        private CubicSpline spline;
        private InverseInterpolation invInterp;
        private MultiDimensionalInterpolation multiInterp;
        private TrigonometricInterpolation trigInterp;

        public ChartForm(CubicSpline spline, InverseInterpolation invInterp, MultiDimensionalInterpolation multiInterp, TrigonometricInterpolation trigInterp)
        {
            this.spline = spline;
            this.invInterp = invInterp;
            this.multiInterp = multiInterp;
            this.trigInterp = trigInterp;

            this.Width = 800;
            this.Height = 600;
            this.Text = "Графики интерполяции";

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);

            tabControl.TabPages.Add(CreateSplineTab());
            tabControl.TabPages.Add(CreateInverseTab());
            tabControl.TabPages.Add(CreateMultiDimTab());
            tabControl.TabPages.Add(CreateTrigonometricTab());
        }

        private TabPage CreateSplineTab()
        {
            TabPage tab = new TabPage("Куб. сплайн");
            Chart chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.ChartAreas.Add(new ChartArea("ChartArea"));
            Series seriesPoints = new Series("Исходные точки");
            seriesPoints.ChartType = SeriesChartType.Point;
            Series seriesSpline = new Series("Сплайн");
            seriesSpline.ChartType = SeriesChartType.Line;

            // Отображаем исходные точки
            for (int i = 0; i < spline.x.Length; i++)
            {
                seriesPoints.Points.AddXY(spline.x[i], spline.y[i]);
            }
            // Строим кривую сплайна
            double minX = spline.x[0];
            double maxX = spline.x[spline.x.Length - 1];
            int numPoints = 200;
            double step = (maxX - minX) / numPoints;
            for (double X = minX; X <= maxX; X += step)
            {
                double Y = spline.Evaluate(X);
                seriesSpline.Points.AddXY(X, Y);
            }
            chart.Series.Add(seriesSpline);
            chart.Series.Add(seriesPoints);
            tab.Controls.Add(chart);
            return tab;
        }

        private TabPage CreateInverseTab()
        {
            TabPage tab = new TabPage("Обратная");
            Chart chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.ChartAreas.Add(new ChartArea("ChartArea"));
            Series seriesPoints = new Series("Исходные точки");
            seriesPoints.ChartType = SeriesChartType.Point;
            Series seriesCurve = new Series("Интерполяция");
            seriesCurve.ChartType = SeriesChartType.Line;

            // Отображаем точки (y как независимая переменная)
            for (int i = 0; i < invInterp.x.Length; i++)
            {
                seriesPoints.Points.AddXY(invInterp.y[i], invInterp.x[i]);
            }
            // Строим интерполированную кривую по диапазону y
            double minY = Math.Min(Math.Min(invInterp.y[0], invInterp.y[1]), invInterp.y[2]);
            double maxY = Math.Max(Math.Max(invInterp.y[0], invInterp.y[1]), invInterp.y[2]);
            int numPoints = 200;
            double step = (maxY - minY) / numPoints;
            for (double Y = minY; Y <= maxY; Y += step)
            {
                double X = invInterp.Evaluate(Y);
                seriesCurve.Points.AddXY(Y, X);
            }
            chart.Series.Add(seriesCurve);
            chart.Series.Add(seriesPoints);
            tab.Controls.Add(chart);
            return tab;
        }

        private TabPage CreateMultiDimTab()
        {
            TabPage tab = new TabPage("Многомерная");
            Chart chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.ChartAreas.Add(new ChartArea("ChartArea"));

            // Для демонстрации выводим точки сетки с подписью значения f(x,y)
            Series series = new Series("Точки сетки");
            series.ChartType = SeriesChartType.Point;
            for (int i = 0; i < multiInterp.yValues.Length; i++)
            {
                for (int j = 0; j < multiInterp.xValues.Length; j++)
                {
                    double fx = multiInterp.fValues[i, j];
                    var pointIndex = series.Points.AddXY(multiInterp.xValues[j], multiInterp.yValues[i]);
                    series.Points[pointIndex].Label = fx.ToString("F2");
                    series.Points[pointIndex].Color = GetColorFromValue(fx);
                }
            }
            chart.Series.Add(series);
            tab.Controls.Add(chart);
            return tab;
        }

        private System.Drawing.Color GetColorFromValue(double value)
        {
            // Простое цветовое отображение (пример)
            int v = (int)(255 * (value - 0.4) / 0.6);
            v = Math.Max(0, Math.Min(255, v));
            return System.Drawing.Color.FromArgb(v, 0, 255 - v);
        }

        private TabPage CreateTrigonometricTab()
        {
            TabPage tab = new TabPage("Тригонометр.");
            Chart chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.ChartAreas.Add(new ChartArea("ChartArea"));
            Series seriesPoints = new Series("Исходные точки");
            seriesPoints.ChartType = SeriesChartType.Point;
            Series seriesTrig = new Series("Интерполяция");
            seriesTrig.ChartType = SeriesChartType.Line;

            // Отображаем исходные точки
            for (int i = 0; i < trigInterp.x.Length; i++)
            {
                seriesPoints.Points.AddXY(trigInterp.x[i], trigInterp.y[i]);
            }
            // Строим интерполированную кривую
            double minX = trigInterp.x[0];
            double maxX = trigInterp.x[trigInterp.x.Length - 1];
            int numPoints = 200;
            double step = (maxX - minX) / numPoints;
            for (double X = minX; X <= maxX; X += step)
            {
                double Y = trigInterp.Interpolate(X);
                seriesTrig.Points.AddXY(X, Y);
            }
            chart.Series.Add(seriesTrig);
            chart.Series.Add(seriesPoints);
            tab.Controls.Add(chart);
            return tab;
        }
    }
}
