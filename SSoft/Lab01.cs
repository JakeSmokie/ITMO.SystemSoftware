using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SSoft {
    [Serializable]
    internal struct Grade {
        public string Name { get; }
        public string Discipline { get; }
        public int GradeValue { get; }
        public int Semester { get; }

        public Grade(string name, string discipline, int gradeValue, int semester) {
            Name = name;
            Discipline = discipline;
            GradeValue = gradeValue;
            Semester = semester;
        }
    }

    public class Lab01 {
        public static void Main(string[] args) {
            Console.WriteLine("╔═══════════════════════╦═══════════════════════╦═══════════════════════╦═══════════════════════╗");
            Console.WriteLine("║ N                     ║ Windows               ║ C                     ║ Windows CopyFile      ║");
            Console.WriteLine("╠═══════════════════════╬═══════════════════════╬═══════════════════════╬═══════════════════════╣");

            for (long i = 1; i < 4; i++) {
                var size = (int) Math.Pow(10, i);

                File.Delete("first");
                GenerateFile(size);

                GC.Collect();

                File.Delete("second");
                var windows = GetTimeOfMethod(WindowsCopy);
                File.Delete("second");
                var cTime = GetTimeOfMethod(CCopy);
                File.Delete("second");
                var copyFile = GetTimeOfMethod(WindowsCopyFileCopy);

                Console.WriteLine($"║ {O(size)} ║ {O(windows)} ║ {O(cTime)} ║ {O(copyFile)} ║");
            }

            Console.WriteLine("╚═══════════════════════╩═══════════════════════╩═══════════════════════╩═══════════════════════╝");
        }

        private static string O<T>(T i) {
            return i.ToString().PadRight(21);
        }

        private static void WindowsCopyFileCopy() {
            File.Copy("first", "second");
        }

        private static void CCopy() {
            using (var inStream = File.OpenRead("first")) {
                var bufferSize = 1024 * 64;

                using (var fileStream = new FileStream("second", FileMode.OpenOrCreate, FileAccess.Write)) {
                    var bytesRead = -1;
                    var bytes = new byte[bufferSize];

                    while ((bytesRead = inStream.Read(bytes, 0, bufferSize)) > 0) {
                        fileStream.Write(bytes, 0, bytesRead);
                        fileStream.Flush();
                    }
                }
            }
        }

        private static void WindowsCopy() {
            File.WriteAllText("second", "");
            var process = Process.Start("xcopy", "first second /Y");
            process.WaitForExit();
        }

        private static TimeSpan GetTimeOfMethod(Action action) {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            action();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private static void GenerateFile(long size) {
            var random = new Random(1000);

            var bufferSize = 1024 * 64;

            using (var writer = new BinaryWriter(File.OpenWrite("first"))) {
                var bytes = new byte[bufferSize];

                for (var i = 0; i < size / bufferSize; i++) {
                    random.NextBytes(bytes);
                    writer.Write(bytes);
                }
            }
        }

        private static byte[] ObjectToByteArray(object obj) {
            if (obj == null) {
                return null;
            }

            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream()) {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}