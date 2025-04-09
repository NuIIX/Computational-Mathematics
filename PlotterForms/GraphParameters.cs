using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotterForms
{
    public struct GraphParameters
    {
        public List<(double x, double y)> Points { get; set; }
        public float MarkerSize { get; set; }
        public float LineWidth { get; set; }
        public Color? Color { get; set; }
        public string Label { get; set; } // Новое свойство для подписи

        public GraphParameters(List<(double x, double y)> points, float markerSize = 5, float lineWidth = 1, Color? color = null, string label = "")
        {
            Points = points ?? throw new ArgumentNullException(nameof(points));
            MarkerSize = markerSize;
            LineWidth = lineWidth;
            Color = color;
            Label = label;
        }
    }
}
