namespace _11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Func<double, double> function = x => x * x - 6 * x;

            Console.WriteLine("Выберите вариант вычисления золотого сечения:");
            Console.WriteLine("1 - по количеству шагов");
            Console.WriteLine("2 - по точности eps");
            Console.Write("Ваш выбор (1/2): ");

            var choice = Console.ReadLine();
            GoldenSectionSearch golden;

            if (choice == "1")
            {
                Console.Write("Введите число шагов: ");
                if (int.TryParse(Console.ReadLine(), out int steps))
                {
                    golden = new GoldenSectionSearch(function, 0, 5, steps);
                }
                else
                {
                    Console.WriteLine("Неверный ввод, используется 4 шага по умолчанию.");
                    golden = new GoldenSectionSearch(function, 0, 5, 4);
                }
            }
            else if (choice == "2")
            {
                Console.Write("Введите eps (например, 1e-6): ");
                if (double.TryParse(Console.ReadLine(), out double eps))
                {
                    golden = new GoldenSectionSearch(function, 0, 5, eps);
                }
                else
                {
                    Console.WriteLine("Неверный ввод, используется eps = 1e-6 по умолчанию.");
                    golden = new GoldenSectionSearch(function, 0, 5, 1e-6);
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор, используется вариант по шагам со 4 шагами.");
                golden = new GoldenSectionSearch(function, 0, 5, 4);
            }

            golden.Compute();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
