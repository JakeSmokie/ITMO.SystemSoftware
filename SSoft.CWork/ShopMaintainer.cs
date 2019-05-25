using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SSoft.CWork.Tools;

namespace SSoft.CWork {
    public class Request {
        public int Amount { get; set; }
        public int Article { get; set; }
        public Action<float, string> ResponseAction { get; set; }

        public void Deconstruct(out int amount, out int article, out Action<float, string> responseAction) {
            amount = Amount;
            article = Article;
            responseAction = ResponseAction;
        }
    }

    public class ShopMaintainer : IEnumerable<Product> {
        private readonly Syncer _syncer;
        private readonly List<Product> _products = new List<Product>();

        public ShopMaintainer(Syncer syncer) {
            _syncer = syncer;
        }

        public void Add(Product product) {
            _products.Add(product);
        }

        public void SellItems(Client client) {
            while (client.Cash > 0 && _products.Any(x => _syncer.Sync(() => x.Quantity > 0))) {
                var product = _products[client.DecisionRandom.Next(_products.Count)];

                if (_syncer.Sync(() => product.Quantity <= 0)) {
                    continue;
                }

                _syncer.Enter();

                if (client.Cash < product.Cost) {
                    product = _products
                        .Where(x => x.Quantity > 0)
                        .OrderBy(x => x.Cost)
                        .FirstOrDefault();

                    if (client.Cash < product.Cost) {
                        break;
                    }

                    client.Cash -= product.Cost;
                    product.Quantity -= 1;
                    continue;
                }

                client.Cash -= product.Cost;
                product.Quantity -= 1;

                _syncer.Exit();
            }
        }

        public IEnumerator<Product> GetEnumerator() {
            return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void PrintProducts() {
            var name = Math.Max(7, _products.Max(x => x.Id.Length));
            var cash = Math.Max(4, _products.Max(x => x.Cost.ToString().Length));
            var quantity = Math.Max(10, _products.Max(x => x.Quantity.ToString().Length));

            var nameLine = '═'.Repeat(name);
            var cashLine = '═'.Repeat(cash);
            var quantityLine = '═'.Repeat(quantity);

            Console.WriteLine($"╔════════╦═{nameLine}═╦═{cashLine}═╦═{quantityLine}═╗");
            Console.WriteLine($"║ №      ║ {O("Артикул", name)} ║ {O("Цена", cash)} ║ {O("Количество", quantity)} ║");
            Console.WriteLine($"╠════════╬═{nameLine}═╬═{cashLine}═╬═{quantityLine}═╣");

            var id = 0;

            foreach (var x in _products) {
                Console.WriteLine($"║ {id++,-6} ║ {O(x.Id, name)} ║ {O(x.Cost, cash)} ║ {O(x.Quantity, quantity)} ║");
            }

            Console.WriteLine($"╚════════╩═{nameLine}═╩═{cashLine}═╩═{quantityLine}═╝");
        }

        private static string O<T>(T i, int x) {
            return i.ToString().PadRight(x);
        }
    }
}