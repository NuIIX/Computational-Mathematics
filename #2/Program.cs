namespace _2
{
    internal class Program
    {
        static void Main()
        {
            NumberWithError x = new NumberWithError(1.1544, 0.003);
            NumberWithError y = new NumberWithError(2.73, 0.033);

            Console.WriteLine($"x = {x}");
            Console.WriteLine($"y = {y}\n");

            NumberWithError sum = x + y;
            NumberWithError diff = x - y;
            NumberWithError prod = x * y;
            NumberWithError quotient = x / y;
            NumberWithError ySquared = y.Pow(2);
            NumberWithError sqrtX = x.Sqrt();
            NumberWithError sinX = x.Sin();
            NumberWithError cosX = x.Cos();
            NumberWithError lnX = x.Ln();
            NumberWithError xPowy= x.Pow(y);

            Console.WriteLine($"x + y = {sum}");
            Console.WriteLine($"x - y = {diff}");
            Console.WriteLine($"x * y = {prod}");
            Console.WriteLine($"x / y = {quotient}");
            Console.WriteLine($"y^2 = {ySquared}");
            Console.WriteLine($"sqrt(x) = {sqrtX}");
            Console.WriteLine($"sin(x) = {sinX}");
            Console.WriteLine($"cos(x) = {cosX}");
            Console.WriteLine($"ln(x) = {lnX}");
            Console.WriteLine($"x^y = {xPowy}");
            Console.WriteLine();
            PrintCalculation();
        }

        static void PrintCalculation()
        {
            NumberWithError x = new NumberWithError(2.0874, 0.0023);
            NumberWithError y = new NumberWithError(0.8704, 0.0014);

            Console.WriteLine($"(x-sqrt(y))^(x/sin(y)) = {(x - y.Sqrt()).Pow(x / y.Sin())}");
        }
    }
}
