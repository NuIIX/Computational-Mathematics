using ScottPlot;
using Shared;
using Color = System.Drawing.Color;
using System.Drawing;

namespace PlotterForms
{
    public partial class Graph : Form
    {
        public Graph()
        {
            InitializeComponent();
            InitializePlot();
        }

        private readonly FormsPlot formsPlot = new FormsPlot();

        private void InitializePlot()
        {
            formsPlot.Dock = DockStyle.Fill;
            Controls.Add(formsPlot);
            CenterAxes();
        }

        private void CenterAxes()
        {
            formsPlot.Plot.XAxis.Label("Ось X", size: 30);
            formsPlot.Plot.YAxis.Label("Ось Y", size: 30);
            formsPlot.Plot.XAxis.TickLabelStyle(fontSize: 30);
            formsPlot.Plot.YAxis.TickLabelStyle(fontSize: 30);
            formsPlot.Plot.AddHorizontalLine(0, Color.Black, 1);
            formsPlot.Plot.AddVerticalLine(0, Color.Black, 1);
        }

        private void CenterAndFit(double[] xs, double[] ys)
        {
            double minX = xs.Min();
            double maxX = xs.Max();
            double minY = ys.Min();
            double maxY = ys.Max();

            double dataWidth = maxX - minX;
            double dataHeight = maxY - minY;

            double paddingRatio = 0.2;

            double xPadding = dataWidth * paddingRatio;
            double yPadding = dataHeight * paddingRatio;

            double finalMinX = minX - xPadding;
            double finalMaxX = maxX + xPadding;

            double finalMinY = minY - yPadding;
            double finalMaxY = maxY + yPadding;

            formsPlot.Plot.SetAxisLimits(finalMinX, finalMaxX, finalMinY, finalMaxY);
        }

        public void PlotPoints(GraphParameters param)
        {
            double[] xs = param.Points.Select(p => p.x).ToArray();
            double[] ys = param.Points.Select(p => p.y).ToArray();

            formsPlot.Plot.AddScatter(xs, ys, markerSize: param.MarkerSize, lineWidth: param.LineWidth, color: param.Color, label: param.Label);

            formsPlot.Plot.AxisScaleLock(false);

            if (param.IsCentered)
            {
                CenterAndFit(xs, ys);
            }

            formsPlot.Render();
        }

        public void PlotSinglePoint(double x, double y, float markerSize = 5)
        {
            formsPlot.Plot.AddPoint(x, y, Color.Red, markerSize);
            formsPlot.Render();
        }

        public void ShowLegendTable()
        {
            var legend = formsPlot.Plot.Legend(location: ScottPlot.Alignment.UpperLeft);

            legend.Font.Size = 25;
            legend.Font.Name = "Segoe UI";

            legend.FillColor = Color.FromArgb(220, Color.White);

            legend.Padding = 10;

            formsPlot.Render();
        }

    }
}
