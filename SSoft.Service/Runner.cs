using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SSoft.Service {
    public static class Runner {
        public static bool IsRunning { get; set; }

        public static async Task Run() {
            IsRunning = true;

            var pics = new[] {
                @"https://www.zastavki.com/pictures/1600x1200/2009/Winter_wallpapers_Winter_mountains_019246_.jpg",
                @"https://images.wallpaperscraft.com/image/waterfall_pond_trees_landscape_79388_2560x1440.jpg",
                @"https://images.wallpaperscraft.com/image/grass_mountains_evening_90428_2560x1600.jpg",
                @"http://s1.1zoom.me/big3/26/357022-svetik.jpg"
            }.Select(s => {
                using (var webClient = new WebClient()) {
                    return Image.FromStream(webClient.OpenRead(s));
                }
            }).ToList();

            while (IsRunning) {
                foreach (var s in pics) {
                    Wallpaper.Set(s, Wallpaper.Style.Stretched);
                    await Task.Delay(10000);
                }
            }
        }
    }
}