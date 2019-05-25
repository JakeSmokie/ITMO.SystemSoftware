using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SSoft.CourseWork {
    internal class Program {
        private static void Main(string[] args) {
            var times = new List<(string, TimeSpan)>();

            var spinLock = new SimpleSpinLock();
            times.Add(("Spin Lock", Bench(spinLock.Enter, spinLock.Leave)));

            var sync = new object();
            times.Add(("Monitor", Bench(() => Monitor.Enter(sync), () => Monitor.Exit(sync))));

            var semSlim = new SemaphoreSlim(1, 1);
            times.Add(("Slim Semaphore", Bench(semSlim.Wait, () => semSlim.Release())));

            var waitLock = new SimpleWaitLock();
            times.Add(("Wait Lock", Bench(() => waitLock.Enter(), () => waitLock.Leave())));

            var semaphore = new Semaphore(1, 1);
            times.Add(("Semaphore", Bench(() => semaphore.WaitOne(), () => semaphore.Release())));

            var mutex = new Mutex();
            times.Add(("Mutex", Bench(() => mutex.WaitOne(), () => mutex.ReleaseMutex())));

            var hybrid = new SimpleHybridLock();
            times.Add(("Hybrid", Bench(() => hybrid.Enter(), () => hybrid.Leave())));

            //            var ms = new ManualResetEventSlim(true);
            //            times.Add(("Slim MRE", Bench(ms.Wait, ms.Set)));

            times.Add(("Concurrent Bag", RunBench2()));
            times.Add(("Concurrent Stack", RunBench3()));
            times.Add(("Blocking Collection", RunBench4()));

            Console.WriteLine("╔═══════════════════════╦═══════════════════════╗");
            Console.WriteLine("║ Type                  ║ Time                  ║");
            Console.WriteLine("╠═══════════════════════╬═══════════════════════╣");

            times.OrderBy(x => x.Item2).ToList()
                .ForEach(t => { Console.WriteLine($"║ {O(t.Item1)} ║ {O(t.Item2)} ║"); });

            Console.WriteLine("╚═══════════════════════╩═══════════════════════╝");
            Console.ReadLine();

            string O<T>(T i) {
                return i.ToString().PadRight(21);
            }
        }

        private static TimeSpan Bench(Action enter, Action leave) {
            var time = Enumerable.Range(0, 1)
                .Select(x => RunBench(enter, leave))
                .Sum(x => x.Ticks);

            return new TimeSpan(time);
        }

        private static TimeSpan RunBench(Action enter, Action leave) {
            var random = new Random(13189739);
            var items = new List<byte>();

            var threads = Enumerable.Range(0, 8)
                .Select(x => new Random(random.Next()))
                .Select(r => {
                    var bytes = new byte[20000];
                    r.NextBytes(bytes);
                    return bytes;
                }).Select(a => new Thread(() => Run(a)))
                .ToList();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            stopwatch.Stop();
            return stopwatch.Elapsed;

            void Run(byte[] bytes) {
                foreach (var b in bytes) {
                    enter?.Invoke();
                    items.Add(b);
                    leave?.Invoke();
                }
            }
        }

        private static TimeSpan RunBench2() {
            var random = new Random(13189739);
            var items = new ConcurrentBag<byte>();

            var threads = Enumerable.Range(0, 8)
                .Select(x => new Random(random.Next()))
                .Select(r => {
                    var bytes = new byte[40000];
                    r.NextBytes(bytes);
                    return bytes;
                }).Select(a => new Thread(() => Run(a)))
                .ToList();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            stopwatch.Stop();
            return stopwatch.Elapsed;

            void Run(byte[] bytes) {
                foreach (var t in bytes) {
                    items.Add(t);
                }
            }
        }

        private static TimeSpan RunBench3() {
            var random = new Random(13189739);
            var items = new ConcurrentStack<byte>();

            var threads = Enumerable.Range(0, 8)
                .Select(x => new Random(random.Next()))
                .Select(r => {
                    var bytes = new byte[40000];
                    r.NextBytes(bytes);
                    return bytes;
                }).Select(a => new Thread(() => items.PushRange(a)))
                .ToList();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private static TimeSpan RunBench4() {
            var random = new Random(13189739);
            var items = new BlockingCollection<byte>();

            var threads = Enumerable.Range(0, 8)
                .Select(x => new Random(random.Next()))
                .Select(r => {
                    var bytes = new byte[40000];
                    r.NextBytes(bytes);
                    return bytes;
                }).Select(a => new Thread(() => Run(a)))
                .ToList();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            stopwatch.Stop();
            return stopwatch.Elapsed;

            void Run(byte[] bytes) {
                foreach (var t in bytes) {
                    items.Add(t);
                }
            }
        }
    }
}