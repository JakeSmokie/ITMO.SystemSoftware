using System.ServiceProcess;
using System.Threading;
using SSoft.Service;

namespace SSoft.NewService {
    public partial class Service1 : ServiceBase {
        public Service1() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            new Thread(Runner.Run).Start();
        }

        protected override void OnStop() {
            Runner.IsRunning = false;
        }
    }
}