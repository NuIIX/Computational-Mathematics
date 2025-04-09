using _6;
using Shared;

namespace _7
{
    public class CubicSplineInterpolation : InterpolationBase
    {
        private double[] _segmentLengths;
        private double[] _slopes;
        private double[] _splineCoefficients;

        public CubicSplineInterpolation(List<(double x, double y)> dataPoints)
            : base(dataPoints)
        {
            int numSegments = PointsCount - 1;

            _segmentLengths = new double[numSegments];
            _slopes = new double[numSegments];
            _splineCoefficients = new double[PointsCount];

            Printer.PrintListAsTable(_points, "Исходные данные");

            for (int i = 0; i < numSegments; i++)
            {
                _segmentLengths[i] = _points[i + 1].x - _points[i].x;
                _slopes[i] = (_points[i + 1].y - _points[i].y) / _segmentLengths[i];
            }

            Console.WriteLine("\nВычисленные длины сегментов (h_i):");
            Console.WriteLine(string.Join("; ", _segmentLengths));

            ComputeSplineCoefficients();
        }

        private void ComputeSplineCoefficients()
        {
            int numEquations = _segmentLengths.Length - 1;
            double[,] matrix = new double[numEquations, numEquations];
            double[] results = new double[numEquations];

            Console.WriteLine("\nФормирование системы уравнений (матрица коэффициентов и правая часть):");

            for (int i = 0; i < numEquations; i++)
            {
                if (i > 0)
                {
                    matrix[i, i - 1] = _segmentLengths[i] / 3;
                }

                matrix[i, i] = 2 * (_segmentLengths[i] + (i < numEquations - 1 ? _segmentLengths[i + 1] : 0)) / 3;

                if (i < numEquations - 1)
                {
                    matrix[i, i + 1] = _segmentLengths[i + 1] / 3;
                }

                results[i] = _slopes[i + 1] - _slopes[i];
            }

            Printer.PrintMatrix(matrix, results);

            GaussianElimination gauss = new GaussianElimination(matrix, results);
            double[] solution = gauss.Solve();

            for (int i = 1; i < _splineCoefficients.Length - 1; i++)
            {
                _splineCoefficients[i] = solution[i - 1];
            }

            Console.WriteLine("\nНайденные коэффициенты M_i:");
            Console.WriteLine(string.Join("; ", _splineCoefficients));
        }

        public double Compute(double x, bool useWrite = true)
        {
            int index = FindSegment(x);

            if (useWrite)
            {
                Console.WriteLine($"\nВычисление S({x}):");
                Console.WriteLine($"Точка x = {x} лежит в промежутке [{_points[index - 1].x}; {_points[index].x}], значит index = {index}");
            }

            double x0 = _points[index - 1].x, x1 = _points[index].x;
            double y0 = _points[index - 1].y, y1 = _points[index].y;
            double m0 = _splineCoefficients[index - 1], m1 = _splineCoefficients[index];
            double h = _segmentLengths[index - 1];

            double[] terms =
            {
                ((x1 - x) * (x1 - x) * (x1 - x) * m0) / (6 * h),
                ((x - x0) * (x - x0) * (x - x0) * m1) / (6 * h),
                ((y0 - (m0 * h * h) / 6) * (x1 - x)) / h,
                ((y1 - (m1 * h * h) / 6) * (x - x0)) / h
            };

            double result = terms.Sum();

            if (useWrite) Console.WriteLine($"S({x}) = {string.Join(" + ", terms)} = {result}");

            return result;
        }

        private int FindSegment(double x)
        {
            for (int i = 1; i < PointsCount; i++)
            {
                if (x <= _points[i].x)
                {
                    return i;
                }
            }

            return PointsCount - 1;
        }
    }
}
