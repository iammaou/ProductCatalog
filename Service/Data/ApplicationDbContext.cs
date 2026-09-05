using Microsoft.EntityFrameworkCore;
using Service.Entities;

namespace Service.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.CategoryId);
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Price);
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.IsActive);
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.StockQuantity);

        modelBuilder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
    }

    public DbSet<Product> Products {get; set;}
    public DbSet<ProductCategory> ProductCategories {get;set;}
}
