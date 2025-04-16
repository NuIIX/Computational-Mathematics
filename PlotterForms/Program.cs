using Shared;

namespace PlotterForms
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var dataPoints = new List<(double x, double y)>
            {
                (1, 2),
                (3, 5),
                (5, 2),
                (7, -1),
                (9, 2)
            };

            var form = new Graph();
            form.PlotPoints(new GraphParameters(dataPoints));

            Application.Run(form);
        }

        public static void ShowGraph(List<GraphParameters> points)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Graph();

            foreach (var data in points)
            {
                form.PlotPoints(data);
            }

            form.ShowLegendTable();

            Application.Run(form);
        }

        public static void ShowPoint(double x, double y)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Graph();
            form.PlotSinglePoint(x, y);

            Application.Run(form);
        }
    }
}
