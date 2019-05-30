using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SSoft.CWork.Tools;

namespace SSoft.CWork {
    internal class Program {
        private static void Main(string[] args) {
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

            var shop = new ShopMaintainer(semaphoreLocker) {
                new Product("Чай Липтон", 20, 30),
                new Product("Колбаса", 40, 20),
                new Product("Булка", 4, 20),
                new Product("Сырок", 1, 10)
            };

            var clients = new[] {
                new Client("Hey", 1000000),
                new Client("Bro", 1000000),
                new Client("Dont", 1000000),
                new Client("Stop", 1000000),
                new Client("Jake", 1000000),
                new Client("Tyler", 2000000),
                new Client("Harry", 6000000),
                new Client("Jeffrey", 8000000)
            };

            PrintClients(clients);
            shop.PrintProducts();

            Parallel.ForEach(clients, shop.SellItems);

            PrintClients(clients);
            shop.PrintProducts();

            Console.ReadLine();
        }

        private static void PrintClients(Client[] clients) {
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