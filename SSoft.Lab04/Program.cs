using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SSoft.Lab04 {
    internal class Program {
        private static void Main(string[] args) {
            if (args.Length == 0) {
                StartAll();
                return;
            }

            switch (args[0]) {
                case "QSort":
                    QSort();
                    break;
                case "MySort":
                    MySort();
                    break;
                case "Search":
                    Search();
                    break;
                default:
                    break;
            }
        }

        private static void StartAll() {
            var args = new[] {"QSort", "MySort", "Search"};

            Console.WriteLine("╔═══════════════════════╦═══════════════════════╦═══════════════════════╦═══════════════════════╦═══════════════════════╗");
            Console.WriteLine("║ Process               ║ Kernel Time           ║ User Time             ║ Total Time            ║ Descriptors Count     ║");
            Console.WriteLine("╠═══════════════════════╬═══════════════════════╬═══════════════════════╬═══════════════════════╬═══════════════════════╣");

            foreach (var s in args) {
                using (var process = new Process {
                    StartInfo = {
                        FileName = "SSoft.Lab04.exe",
                        Arguments = s,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                }) {
                    process.Start();
                    var count = process.HandleCount;
                    process.WaitForExit();

                    var kernel = process.PrivilegedProcessorTime;
                    var user = process.UserProcessorTime;
                    var total = process.TotalProcessorTime;
                    var id = process.Id;

                    Console.WriteLine($"║ {O(id)} ║ {O(kernel)} ║ {O(user)} ║ {O(total)} ║ {O(count)} ║");
                }
            }

            Console.WriteLine("╚═══════════════════════╩═══════════════════════╩═══════════════════════╩═══════════════════════╩═══════════════════════╝");

            string O<T>(T i) {
                return i.ToString().PadRight(21);
            }
        }

        private static void Search() {
            var bytes = Gen().ToHashSet();

            for (byte i = 0; i < 255; i++) {
                bytes.TryGetValue(i, out var actual);
            }

            foreach (var b in bytes) {
                bytes.TryGetValue(b, out var actual);
            }
        }

        private static void MySort() {
            var set = new SortedSet<byte>(Gen());
            var c = set.Count;
        }

        private static void QSort() {
            var a = Gen();
            Array.Sort(a);
        }

        private static byte[] Gen() {
            var bytes = new byte[1000000];
            new Random(102313).NextBytes(bytes);
            return bytes;
        }
    }
}