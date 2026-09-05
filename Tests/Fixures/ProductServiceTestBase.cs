using Service.Data;
using Service.Entities;
using Tests.Helpers;

namespace Tests.Fixures;

public abstract class ProductServiceTestBase : IDisposable
{
    protected ApplicationDbContext Db { get; private set; }
    protected Guid Category1 { get; private set; }
    protected Guid Category2 { get; private set; }

    protected ProductServiceTestBase()
    {
        Db = TestDbContextFactory.Create();
    }

    protected async Task SeedAsync()
    {
        var catA = new ProductCategory { Name = "Electronics", Description = "Gadgets" };
        var catB = new ProductCategory { Name = "Books", Description = "Read" };
        
        Db.ProductCategories.AddRange(catA, catB);
        await Db.SaveChangesAsync();

        Category1 = catA.Id;
        Category2 = catB.Id;

        Db.Products.AddRange(
            new Product { Name = "Laptop", Price = 1000m, StockQuantity = 5, IsActive = true, CategoryId = catA.Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new Product { Name = "Phone", Price = 500m, StockQuantity = 0, IsActive = true, CategoryId = catA.Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new Product { Name = "Book", Price = 20m, StockQuantity = 10, IsActive = false, CategoryId = catB.Id, CreatedAt = DateTime.UtcNow.AddDays(-1) }
        );
        await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db?.Dispose();
        GC.SuppressFinalize(this);
    }
}