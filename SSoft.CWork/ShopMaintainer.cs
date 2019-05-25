using System;
using System.Collections.Generic;
using System.Linq;

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

    public class ShopMaintainer : IDisposable {
        private readonly ShopContext _context = new ShopContext();
        private readonly Syncer _syncer;

        public Queue<Request> Requests { get; } = new Queue<Request>();

        public ShopMaintainer(Syncer syncer) {
            _syncer = syncer;
        }

        public void Dispose() {
            _context?.Dispose();
        }

        public void Run() {
            while (true) {
                while (AnyRequests()) {
                    _syncer.Enter();
                    var (amount, article, action) = Requests.Dequeue();

                    var product = _context.Products
                        .FirstOrDefault(x => x.Id == article);

                    if (product == null) {
                        Console.WriteLine("No");
                    }
                    else if (product.Quantity < amount) {
                        Console.WriteLine("Less");
                    }
                    else {
                        var cost = amount * product.Cost;

                        product.Quantity -= amount;
                        _context.SaveChanges();

                        Console.WriteLine($"Cost: {cost}");
                    }
                }

                _syncer.Exit();
            }
        }

        private bool AnyRequests() {
            _syncer.Enter();
            var r = Requests.Count > 0;
            _syncer.Exit();
            return r;
        }

        public void AddRequest(int amount, int article) {
            _syncer.Enter();
            Requests.Enqueue((amount, article));
            _syncer.Exit();
        }
    }
}