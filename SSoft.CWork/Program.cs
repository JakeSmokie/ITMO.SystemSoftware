using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSoft.CWork {
    class Program {
        static void Main(string[] args) {
            var syncObject = new object();
            var semaphore = new SemaphoreSlim(1, 1);

            var criticalSectionsLocker = new Syncer(
                () => Monitor.Enter(syncObject),
                () => Monitor.Exit(syncObject)
            );

            var semaphoreLocker = new Syncer(
                () => semaphore.Wait(),
                () => semaphore.Release()
            );

            var shop = new ShopMaintainer(criticalSectionsLocker);



            shop.Run();
        }
    }
}