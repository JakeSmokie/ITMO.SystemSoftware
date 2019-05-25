using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace SSoft.Lab02 {
    public class Lab02 {
        public static void Main(string[] args) {
            var sizes = new List<int> {
                10,
                50,
                100,
                200,
                500, 1000, 1500, 2000, 2200, 2400, 2600, 2800, 3000
            };

            Console.WriteLine("╔═══════════════════════╦═══════════════════════╦═══════════════════════╦═══════════════════════╗");
            Console.WriteLine("║ N                     ║ Heap                  ║ View                  ║ Base pointers         ║");
            Console.WriteLine("╠═══════════════════════╬═══════════════════════╬═══════════════════════╬═══════════════════════╣");

            sizes.ForEach(size => {
                var array = GenFile(size);

                var heapTime = GetTimeOfMethod(() => Heap(array));
                var ptrMethodTime = GetTimeOfMethod(() => PtrMethod(size));
                var viewMethod = GetTimeOfMethod(() => ViewMethod(size));

                Console.WriteLine($"║ {O(size)} ║ {O(heapTime)} ║ {O(viewMethod)} ║ {O(ptrMethodTime)} ║");
            });

            Console.WriteLine("╚═══════════════════════╩═══════════════════════╩═══════════════════════╩═══════════════════════╝");
        }

        private static string O<T>(T i) {
            return i.ToString().PadRight(21);
        }

        private static void ViewMethod(int size) {
            using (var mmf = MemoryMappedFile.CreateFromFile($"example_view_{size}", FileMode.Open))
            using (var a = mmf.CreateViewAccessor()) {
                var array = new byte[size];
                a.ReadArray(0, array, 0, size);

                Heap(array);
                a.WriteArray(0, array, 0, size);

                a.Dispose();
                mmf.Dispose();
            }
        }

        private static void PtrMethod(int size) {
            using (var mmf = MemoryMappedFile.CreateFromFile($"example_ptr_{size}", FileMode.Open)) {
                var a = mmf.CreateViewAccessor();

                for (var i = 0; i < size - 1; i++) {
                    for (var j = 0; j < size - 1; j++) {
                        var l = a.ReadByte(j);
                        var r = a.ReadByte(j + 1);

                        if (l > r) {
                            a.Write(j, r);
                            a.Write(j + 1, l);
                        }
                    }
                }
            }
        }

        private static void Heap(byte[] array) {
            for (var i = 0; i < array.Length - 1; i++) {
                for (var j = 0; j < array.Length - 1; j++) {
                    if (array[j] > array[j + 1]) {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }
        }

        private static TimeSpan GetTimeOfMethod(Action action) {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private static byte[] GenFile(int size) {
            var array = new byte[size];
            new Random(164809).NextBytes(array);

            File.WriteAllBytes($"example_view_{size}", array);
            File.WriteAllBytes($"example_ptr_{size}", array);
            return array;
        }
    }
}