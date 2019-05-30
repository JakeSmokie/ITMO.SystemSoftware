namespace SSoft.CWork {
    public class Product {
        private int _quantity;


        public Product(string id, decimal cost, int quantity) {
            Id = id;
            Cost = cost;
            Quantity = quantity;
        }

        public string Id { get; }
        public decimal Cost { get; }
        public Syncer Syncer { get; set; }

        public int Quantity {
            get { return _quantity; }
            set {
                if (value < 0) {
                    return;
                }

                _quantity = value;
            }
        }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, {nameof(Cost)}: {Cost}, {nameof(Quantity)}: {Quantity}";
        }
    }
}