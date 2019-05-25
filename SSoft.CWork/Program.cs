using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SSoft.CWork.Tools;

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

            var shop = new ShopMaintainer(criticalSectionsLocker) {
                new Product("Чай Липтон", 20, 300),
                new Product("Колбаса", 40, 2000),
                new Product("Булка", 4, 20000),
                new Product("Сырок", 1, 1000000),
            };

            var clients = new[] {
                new Client("Hey", 1000000),
                new Client("Bro", 1000000),
                new Client("Dont", 1000000),
                new Client("Stop", 1000000),
                new Client("Jake", 1000000),
                new Client("Tyler", 2000000),
                new Client("Harry", 6000000),
                new Client("Jeffrey", 8000000),
            };

            PrintClients(clients);
            shop.PrintProducts();

            var tasks = clients
                .Select(client => Task.Run(() => shop.SellItems(client)))
                .ToArray();

            Task.WaitAll(tasks);

            PrintClients(clients);
            shop.PrintProducts();
        }

        static void PrintClients(Client[] clients) {
            var name = Math.Max(3, clients.Max(x => x.Name.Length));
            var cash = Math.Max(6, clients.Max(x => x.Cash.ToString().Length));

            var nameLine = '═'.Repeat(name);
            var cashLine = '═'.Repeat(cash);

            Console.WriteLine($"╔═════╦═{nameLine}═╦═{cashLine}═╗");
            Console.WriteLine($"║ №   ║ {O("Имя", name)} ║ {O("Деньги", cash)} ║");
            Console.WriteLine($"╠═════╬═{nameLine}═╬═{cashLine}═╣");

            var id = 0;

            foreach (var x in clients) {
                Console.WriteLine($"║ {id++,-3} ║ {O(x.Name, name)} ║ {O(x.Cash, cash)} ║");
            }

            Console.WriteLine($"╚═════╩═{nameLine}═╩═{cashLine}═╝");
        }

        private static string O<T>(T i, int x) {
            return i.ToString().PadRight(x);
        }
    }
}