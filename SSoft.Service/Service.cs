using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace SSoft.Service {
    public class Service : ServiceBase {
        private readonly IContainer components = new Container();

        public Service() {
            ServiceName = $"AAA {MyInstaller.DateTime}";
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args) {
//            Runner.Run();
        }

        protected override void OnStop() {
            Runner.IsRunning = false;
        }
    }
}