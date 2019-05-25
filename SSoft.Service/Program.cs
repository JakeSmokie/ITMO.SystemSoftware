using System;
using System.Collections;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SSoft.Service {
    internal static class Program {
        private static void Main(string[] args) {
            Console.WriteLine("====");

            if (args.Contains("console")) {
                Task.WaitAll(Runner.Run());
                return;
            }

            if (args.Contains("i")) {
                Install();
                return;
            }

            if (args.Contains("u")) {
                Uninstall();
                return;
            }
        }

        private static void Install() {
            Console.WriteLine("Installing...");

            var installer = new TransactedInstaller {
                Context = new InstallContext("log.log", Array.Empty<string>())
            };

            installer.Installers.Add(new MyInstaller());
            installer.Install(new Hashtable());

            Console.WriteLine("Installed...");
        }

        private static void Uninstall() {
            Console.WriteLine("Uninstalling...");

            new TransactedInstaller().Uninstall(null);
            Console.WriteLine("Uninstalled...");
        }
    }
}