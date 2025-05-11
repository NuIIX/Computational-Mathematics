namespace _11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Func<double, double> function = x => x * x - 6 * x;

            var golden = new GoldenSectionSearch(function, 0, 5, 4);

            golden.Compute();
        }
    }
}
