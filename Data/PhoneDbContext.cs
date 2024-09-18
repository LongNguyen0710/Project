using Microsoft.EntityFrameworkCore;
using MvcPhone.Models;
namespace MvcPhone.Data;
using MvcPhone.Models;

public class PhoneDbContext : DbContext
{
    public PhoneDbContext(DbContextOptions<PhoneDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category) // Mỗi Product có một Category
            .WithOne(c => c.Product) // Mỗi Category có một Product
            .HasForeignKey<Product>(p => p.CategoryId); // CategoryId là khóa ngoại trong Product

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductBill>()
          .HasKey(pb => new { pb.ProductId, pb.BillId });

        modelBuilder.Entity<ProductBill>()
            .HasOne(pb => pb.Product)
            .WithMany(p => p.ProductBills)
            .HasForeignKey(pb => pb.ProductId);

        modelBuilder.Entity<ProductBill>()
            .HasOne(pb => pb.Bill)
            .WithMany(b => b.ProductBills)
            .HasForeignKey(pb => pb.BillId);
    }

    public DbSet<Login> Logins { get; set; } = null!;
    public DbSet<Product> Product { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<MvcPhone.Models.Bill> Bill { get; set; } = default!;

}