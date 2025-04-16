using System.Reflection.Emit;
using Color = System.Drawing.Color;

namespace Shared
{
    public struct GraphParameters
    {
        public List<(double x, double y)> Points { get; set; }
        public float MarkerSize { get; set; }
        public float LineWidth { get; set; }
        public Color? Color { get; set; }
        public bool IsCentered { get; set; }
        public string Label { get; set; }

        public GraphParameters(List<(double x, double y)> points, float markerSize = 5, float lineWidth = 1, Color? color = null, bool isCentered = false, string label = "")
        {
            Points = points ?? throw new ArgumentNullException(nameof(points));
            MarkerSize = markerSize;
            LineWidth = lineWidth;
            Color = color;
            IsCentered = isCentered;
            Label = label;
        }
    }
}
