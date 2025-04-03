using System;
using System.Windows.Forms;

namespace InterpolationApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Результаты интерполяционных алгоритмов");
            Console.WriteLine("======================================");

            CubicSpline spline = new CubicSpline();
            spline.InitializeData();
            double s2 = spline.Evaluate(2);
            double s4 = spline.Evaluate(4);
            Console.WriteLine("Кубическая интерполяция сплайнами:");
            Console.WriteLine($"S(2) = {s2:F4}");
            Console.WriteLine($"S(4) = {s4:F4}");
            Console.WriteLine();

            InverseInterpolation invInterp = new InverseInterpolation();
            invInterp.InitializeData();
            double invResult = invInterp.Evaluate(0);
            Console.WriteLine("Обратная интерполяция (формула Лагранжа):");
            Console.WriteLine($"P(0) = {invResult:F4}");
            Console.WriteLine();

            MultiDimensionalInterpolation multiInterp = new MultiDimensionalInterpolation();
            multiInterp.InitializeData();
            double multiResult = multiInterp.Interpolate(1.0, 1.0);
            Console.WriteLine("Многомерная интерполяция:");
            Console.WriteLine($"f(1, 1) = {multiResult:F4}");
            Console.WriteLine();

            TrigonometricInterpolation trigInterp = new TrigonometricInterpolation();
            trigInterp.InitializeData();
            double trigResult = trigInterp.Interpolate(1.5);
            Console.WriteLine("Тригонометрическая интерполяция:");
            Console.WriteLine($"y(1.5) = {trigResult:F4}");
            Console.WriteLine();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChartForm(spline, invInterp, multiInterp, trigInterp));
        }
    }
}
