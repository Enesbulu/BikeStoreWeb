using BikeStoreWeb.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace BikeStoreWeb.Data.Context
{
    public class BikeStoreDbContext : DbContext
    {
        public BikeStoreDbContext(DbContextOptions<BikeStoreDbContext> options)
         : base(options)
        {
        }

        //Tablolar
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        //DB Ek Ayarlar
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Decimal alanlar(para birimleri) için ayarlama
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Price).HasColumnType("decimal(18,2)");
            base.OnModelCreating(modelBuilder);
        }
    }
}