using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace SSoft.NewService {
    [RunInstaller(true)]
    public partial class Installer1 : Installer {
        private readonly ServiceProcessInstaller processInstaller;
        private readonly ServiceInstaller serviceInstaller;

        public Installer1() {
            InitializeComponent();
            processInstaller = new ServiceProcessInstaller {Account = ServiceAccount.LocalSystem};

            serviceInstaller = new ServiceInstaller {
                StartType = ServiceStartMode.Manual,
                ServiceName = "AAA Desktop Slide Show"
            };

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}