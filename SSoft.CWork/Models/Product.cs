using Microsoft.EntityFrameworkCore;

namespace SSoft.CWork {
    public class Product {
        private int _quantity;
        public string Id { get; }
        public decimal Cost { get; }
        public Syncer Syncer { get; set; }

        public int Quantity {
            get { return Syncer.Sync(() => _quantity); }
            set {
                if (value < 0) {
                    return;
                }

                Syncer?.Enter();
                _quantity = value;
                Syncer?.Exit();
            }
        }



        public Product(string id, decimal cost, int quantity) {
            Id = id;
            Cost = cost;
            Quantity = quantity;
        }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, {nameof(Cost)}: {Cost}, {nameof(Quantity)}: {Quantity}";
        }
    }
}