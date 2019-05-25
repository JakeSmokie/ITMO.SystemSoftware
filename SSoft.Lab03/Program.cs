using System.Runtime.InteropServices;

namespace SSoft.Lab03 {
    internal class Program {
        [DllImport(@"C:\dos\eighth\bin\eight.dll")]
        private static extern void Lab07();

        private static void Main(string[] args) {
            Lab01.Main(null);
            Lab02.Lab02.Main(null);
            Lab07();
        }
    }
}