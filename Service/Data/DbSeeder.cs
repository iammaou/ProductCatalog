using Microsoft.EntityFrameworkCore;
using Service.Entities;

namespace Service.Data;

public class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        if(await dbContext.ProductCategories.AnyAsync()) return;

        var categories = new List<ProductCategory>
        {
            new() { Name = "Electronics", Description = "Gadgets and devices" },
            new() { Name = "Books", Description = "Printed and digital books" },
            new() { Name = "Clothing", Description = "Apparel and accessories" }
        };

        dbContext.ProductCategories.AddRange(categories);
        await dbContext.SaveChangesAsync();

        var products = new List<Product>
        {
            new()
            {
                Name = "Laptop",
                Price = 999.99m,
                StockQuantity = 25,
                IsActive = true,
                CategoryId = categories[0].Id,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "C# Book",
                Price = 39.99m,
                StockQuantity = 0,
                IsActive = true,
                CategoryId = categories[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-5).Date
            },
            new()
            {
                Name = "T-shirt",
                Price = 11.99m,
                StockQuantity = 1,
                IsActive = false,
                CategoryId = categories[2].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
        };

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();
    }
}
