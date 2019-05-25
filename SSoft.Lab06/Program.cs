using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SSoft.Lab06 {
    internal class Program {
        private static void Main(string[] args) {
            var mutex = new Mutex(true, "ssoft_lab06_", out var created);

            if (!created) {
                Console.WriteLine("Данная программа уже запущена.");
                return;
            }

            var l = Console.CursorLeft;
            var r = Console.CursorTop;
            var c = Console.BackgroundColor;

            Run();
            Console.Read();

            Console.CursorLeft = l;
            Console.CursorTop = r;
            Console.BackgroundColor = c;
        }

        private static async void Run() {
            while (true) {
                foreach (var color in typeof(ConsoleColor).GetEnumValues().Cast<ConsoleColor>()) {
                    Console.CursorLeft = 0;
                    Console.CursorTop = 0;
                    Console.BackgroundColor = color;

//                    Console.WriteLine();
                    Console.Write(new string(Enumerable.Repeat(' ', Console.WindowWidth).ToArray()));
                    await Task.Delay(1000);
                }
            }
        }
    }
}