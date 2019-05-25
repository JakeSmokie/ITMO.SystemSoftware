using Microsoft.EntityFrameworkCore;

namespace SSoft.CWork {
    public class ShopContext : DbContext {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=shop.db");
        }
    }

    public class Product {
        public int Id { get; set; }
        public float Cost { get; set; }
        public int Quantity { get; set; }
    }
}