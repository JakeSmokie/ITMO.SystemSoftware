using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SSoft.CWork.Tools;

namespace SSoft.CWork {
    public class ShopMaintainer : IEnumerable<Product> {
        private readonly List<Product> _products = new List<Product>();
        private readonly Syncer _syncer;

        public ShopMaintainer(Syncer syncer) {
            _syncer = syncer;
        }

        public IEnumerator<Product> GetEnumerator() {
            return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(Product product) {
            product.Syncer = _syncer;
            _products.Add(product);
        }

        public void SellItems(Client client) {
            while (client.Cash > 0 && _products.Any(x => x.Quantity > 0)) {
                _syncer.Enter();
                var product = _products[client.DecisionRandom.Next(_products.Count)];

                if (product.Quantity <= 0) {
                    _syncer.Exit();
                    continue;
                }

                if (client.Cash < product.Cost) {
                    product = _products
                        .Where(x => x.Quantity > 0)
                        .OrderBy(x => x.Cost)
                        .FirstOrDefault();

                    if (client.Cash < product.Cost) {
                        _syncer.Exit();
                        break;
                    }

                    if (product.Quantity > 0) {
                        client.Cash -= product.Cost;
                        product.Quantity -= 1;
                    }

                    _syncer.Exit();
                    continue;
                }

                if (product.Quantity > 0) {
                    client.Cash -= product.Cost;
                    product.Quantity -= 1;
                }

                _syncer.Exit();
            }
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