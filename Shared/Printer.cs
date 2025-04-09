using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class Printer
    {
        public static void PrintListAsTable(List<(double, double)> list, string title = "")
        {
            if (title.Length > 0)
            {
                Console.WriteLine(title + "\n");
            }

            Console.WriteLine("{0,-15} {1,-15}", "X", "Y");
            Console.WriteLine(new string('-', 30));

            foreach (var (x, y) in list)
            {
                Console.WriteLine("{0,-15:F2} {1,-15:F6}", x, y);
            }

            Console.WriteLine();
        }

        public static void PrintMatrix(double[,] matrix, double[] results, string title = "")
        {
            int size = results.Length;

            if (title.Length > 0)
            {
                Console.WriteLine("\n" + title);
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write($"{matrix[i, j],8:F4} ");
                }

                Console.WriteLine($" | {results[i],8:F4}");
            }
        }
    }
}
