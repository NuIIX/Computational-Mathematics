using System.Numerics;
using Shared;

namespace _8
{
    public class TrigonometricInterpolation : InterpolationBase<Complex>
    {
        private readonly int _n;
        private readonly double _t;
        private readonly double _h;

        public TrigonometricInterpolation(List<(double x, double y)> points) : base(points)
        {
            _n = points.Count;
            _t = points.Last().x - points.First().x + 1;
            _h = points[1].x - points[0].x;
        }

        public override Complex Compute(double x, bool useWrite = true)
        {
            if (useWrite) Console.WriteLine($"n = {_n}\nT = {_t}\nh = {_h}\n");

            double x0 = _points[0].x;
            double normalizedX = (x - x0) / _h;

            if (useWrite) Console.WriteLine($"Найти y({x}) => x0 = {x0}, нормализованный x = {normalizedX}\n");

            Dictionary<int, Complex> A = new Dictionary<int, Complex>();

            for (int j = -_n / 2; j <= _n / 2 - 1; j++)
            {
                Complex aj = 0;

                for (int k = 0; k < _n; k++)
                {
                    double yk = _points[k].y;
                    double angle = -2 * Math.PI * j * k / _n;
                    aj += yk * Complex.Exp(Complex.ImaginaryOne * angle);
                }

                A[j] = aj;

                if (useWrite) Console.WriteLine($"A_{j} = {FormatComplex(aj)}");
            }

            if (useWrite) Console.WriteLine();

            Complex sum = 0;

            for (int j = -_n / 2; j <= _n / 2 - 1; j++)
            {
                double angle = 2 * Math.PI * j * normalizedX / _n;
                Complex exp = Complex.Exp(Complex.ImaginaryOne * angle);
                Complex term = A[j] * exp;

                if (useWrite) Console.WriteLine($"A_{j} * exp(2 * pi * i * {j} * {normalizedX} / {_n}) = {FormatComplex(term)}");

                sum += term;
            }

            Complex result = sum / _n;

            if (useWrite) Console.WriteLine($"\ny({x}) = {FormatComplex(result)}");

            return result;
        }

        private string FormatComplex(Complex number)
        {
            double re = Math.Round(number.Real, 4);
            double im = Math.Round(number.Imaginary, 4);

            string reStr = re != 0 ? re.ToString() : "";
            string imStr = "";

            if (im != 0)
            {
                string imVal = (Math.Abs(im) == 1 ? "" : Math.Abs(im).ToString());
                imStr = $"{(im < 0 ? "- " : (re != 0 ? "+ " : ""))}{imVal}i";
            }

            if (re == 0 && im == 0)
            {
                return "0";
            }

            if (re == 0) 
            {
                return imStr.Replace("+-", "-");
            }

            if (im == 0)
            {
                return reStr;
            }

            return $"{reStr} {imStr}";
        }
    }
}
