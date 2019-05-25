using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SSoft.Lab05 {
    internal class Program {
        private static void Main(string[] args) {
            StartAll();
        }

        private static void StartAll() {
            Console.WriteLine("╔═══════════════════════╦═══════════════════════╦═══════════════════════╗");
            Console.WriteLine("║ Thread                ║ Time                  ║ ID                    ║");
            Console.WriteLine("╠═══════════════════════╬═══════════════════════╬═══════════════════════╣");

            var threads = new Action[] {QSort, MySort, Search}
                .Select(a => new Thread(() => Runner(a)))
                .ToList();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            Console.WriteLine("╚═══════════════════════╩═══════════════════════╩═══════════════════════╝");
            Console.ReadLine();
        }

        private static void Runner(Action action) {
            var name = action.Method.Name;
            var time = GetTimeOfMethod(action);

            Console.WriteLine($"║ {O(name)} ║ {O(time)} ║ {O(AppDomain.GetCurrentThreadId())} ║");

            string O<T>(T i) {
                return i.ToString().PadRight(21);
            }
        }

        private static TimeSpan GetTimeOfMethod(Action action) {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();
            return stopwatch.Elapsed;
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
            var bytes = new byte[100000];
            new Random(102313).NextBytes(bytes);
            return bytes;
        }
    }
}