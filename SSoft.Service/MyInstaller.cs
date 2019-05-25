using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace SSoft.Service {
    [RunInstaller(true)]
    public class MyInstaller : Installer {
        private IContainer components = new Container();
        public static readonly DateTime DateTime = DateTime.Now;

        public MyInstaller() {
            var spi = new ServiceProcessInstaller();
            var si = new ServiceInstaller();

            spi.Account = ServiceAccount.LocalSystem;
            si.StartType = ServiceStartMode.Manual;
            si.ServiceName = $"AAA {DateTime}";

            Installers.Add(spi);
            Installers.Add(si);
        }
    }
}