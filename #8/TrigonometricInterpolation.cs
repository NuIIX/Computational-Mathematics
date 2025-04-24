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

            if (_n > 1)
            {
                _h = points[1].x - points[0].x;
                _t = _n * _h;
            }
            else if (_n == 1)
            {
                _h = 0;
                _t = 0;
            }
            else
            {
                _h = 0;
                _t = 0;
            }
        }

        public override Complex Compute(double x, bool useWrite = true)
        {
            if (_n == 0) return Complex.NaN;
            if (_n == 1) return new Complex(_points[0].y, 0);

            double x0 = _points[0].x;
            double x_relative = x - x0;

            if (useWrite) Console.WriteLine($"n = {_n}\nT = {_t}\nh = {_h}\n");
            if (useWrite) Console.WriteLine($"Найти y({x}) => x0 = {x0}, относительный x = {x_relative}\n");

            Dictionary<int, Complex> A = new Dictionary<int, Complex>();

            int M = _n / 2;
            int j_start = -M;
            int j_end = (_n % 2 != 0) ? M : M - 1;

            for (int j = j_start; j <= j_end; j++)
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

            for (int j = j_start; j <= j_end; j++)
            {
                double angle = 2 * Math.PI * j * x_relative / _t;

                Complex exp = Complex.Exp(Complex.ImaginaryOne * angle);
                Complex term = A[j] * exp;

                if (useWrite) Console.WriteLine($"A_{j} * exp(2 * pi * i * {j} * {x_relative} / {_t}) = {FormatComplex(term)}");

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

            string reStr = re.ToString();
            string imStr = "";

            if (Math.Abs(im) > 1e-9)
            {
                string imVal = (Math.Abs(im) == 1 ? "" : Math.Abs(im).ToString());
                imStr = $"{(im < 0 ? "- " : (re != 0 ? "+ " : ""))}{imVal}i";
            }

            if (re == 0 && Math.Abs(im) < 1e-9)
            {
                return "0";
            }

            if (re == 0)
            {
                return imStr.TrimStart('+').Replace("- -", "+ ").Replace("+-", "-");
            }

            if (Math.Abs(im) < 1e-9)
            {
                return reStr;
            }

            if (re != 0 && imStr.StartsWith("+"))
            {
                return $"{reStr} {imStr}";
            }
            if (re != 0 && imStr.StartsWith("-"))
            {
                return $"{reStr} {imStr.Replace("- ", "-")}";
            }
            return $"{reStr}{imStr}";
        }
    }
}