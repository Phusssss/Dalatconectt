using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProductStore> ProductStores { get; set; }



        // Cấu hình mối quan hệ nhiều-nhiều giữa Product và Store
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Thiết lập mối quan hệ giữa Product và Store qua ProductStore
            modelBuilder.Entity<ProductStore>()
                .HasKey(ps => new { ps.ProductId, ps.StoreId });

            modelBuilder.Entity<ProductStore>()
                .HasOne(ps => ps.Product)
                .WithMany() // Không cần truy cập vào ProductStores trong Product
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductStore>()
                .HasOne(ps => ps.Store)
                .WithMany(s => s.ProductStores) // Sử dụng ProductStores trong Store
                .HasForeignKey(ps => ps.StoreId);
        }

    }
}
