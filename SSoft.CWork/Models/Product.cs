using Microsoft.EntityFrameworkCore;

namespace SSoft.CWork {
    public class Product {
        public string Id { get; }
        public decimal Cost { get; }
        public int Quantity { get; set; }

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