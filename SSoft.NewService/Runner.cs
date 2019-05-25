using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SSoft.Service {
    public static class Runner {
        public static bool IsRunning { get; set; }

        public static void Run() {
            IsRunning = true;
            var file = @"C:\processes.log";

            while (IsRunning) {
                File.AppendAllText(file, $"\n* {DateTime.Now} *\n");

                try {
                    foreach (var p in Process.GetProcesses().Where(x => x.Id != 0)) {
                        File.AppendAllText(file, $"\n * Process: {p.ProcessName} [{p.Id}]\n");
                        File.AppendAllText(file, $" * Handles count: {p.HandleCount}\n");
                        File.AppendAllText(file, $" * Time: {p.UserProcessorTime} / {p.PrivilegedProcessorTime} / {p.TotalProcessorTime}\n");
                        File.AppendAllText(file, $" * Memory: {p.PagedMemorySize64 / 1024.0 / 1024.0} MB\n");
                    }
                }
                catch {
                    // ignored
                }

                Thread.Sleep(TimeSpan.FromMinutes(10));
            }

//
//
//            var pics = new[] {
//                @"https://www.zastavki.com/pictures/1600x1200/2009/Winter_wallpapers_Winter_mountains_019246_.jpg",
//                @"https://images.wallpaperscraft.com/image/waterfall_pond_trees_landscape_79388_2560x1440.jpg",
//                @"https://images.wallpaperscraft.com/image/grass_mountains_evening_90428_2560x1600.jpg",
//                @"http://s1.1zoom.me/big3/26/357022-svetik.jpg"
//            }.Select(s => {
//                using (var webClient = new WebClient()) {
//                    return Image.FromStream(webClient.OpenRead(s));
//                }
//            }).ToList();
//
//            while (IsRunning) {
//                foreach (var s in pics) {
//                    Wallpaper.Set(s, Wallpaper.Style.Stretched);
//                    Thread.Sleep(1000);
//                }
//            }
        }
    }
}