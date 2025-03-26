using ScottPlot;
using System.Drawing;

namespace Plotter
{
    public class GraphPlotter
    {
        private Size _size;
        private Plot _plt;
        private List<double> _xData;
        private List<double> _yData;
        private string _title;
        private string _xLabel;
        private string _yLabel;

        public GraphPlotter(Size size)
        {
            _size = size;
            _plt = new Plot();
            _xData = new List<double>();
            _yData = new List<double>();
            _title = "График";
            _xLabel = "X";
            _yLabel = "Y";
        }

        public void SetTitle(string title)
        {
            _title = title;
        }

        public void SetLabels(string xLabel, string yLabel)
        {
            _xLabel = xLabel;
            _yLabel = yLabel;
        }

        public void AddData(List<double> x, List<double> y, string label = "Данные", bool scatter = false)
        {
            if (x.Count != y.Count)
            {
                throw new ArgumentException("Массивы X и Y должны быть одинакового размера.");
            }

            _xData = x;
            _yData = y;

            if (scatter)
            {
                _plt.Add.Scatter(x.ToArray(), y.ToArray());
            }
            else
            {
                _plt.Add.Signal(x.ToArray());
            }
        }

        public void AddPoint(double x, double y, string label = "Точка", int size = 10)
        {
            _plt.Add.Scatter([x], new double[] { y });
        }

        public void Save(string filePath = "graph.png")
        {
            _plt.Title(_title);
            _plt.XLabel(_xLabel);
            _plt.YLabel(_yLabel);
            _plt.Add.Legend();

            _plt.SavePng(filePath, _size.Width, _size.Height);
            Console.WriteLine($"График сохранен как {filePath}");
        }
    }
}