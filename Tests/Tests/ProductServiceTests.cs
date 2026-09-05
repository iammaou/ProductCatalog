using System;
using Service.DTO;
using Service.Services;
using Tests.Fixures;

namespace Tests.Tests;

public class ProductServiceTests : ProductServiceTestBase
{
    
    [Fact]
    public async Task GetAllProductsAsync_WithNoFilters_ReturnsAllProducts()
    {
        await SeedAsync();
        var service = new ProductService(Db);
        var expectedCount = Db.Products.Count();

        var result = await service.GetAllProductsAsync(new ProductQueryParameters
        {
            Page = 1,
            PageSize = 10
        });

        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.TotalCount);
        Assert.True(result.TotalCount > 0);
    }

    [Fact]
    public async Task GetAllProductsAsync_FiltersByCategory_ReturnsOnlyOneCategory()
    {
        await SeedAsync();
        var service = new ProductService(Db);

        var result = await service.GetAllProductsAsync(new ProductQueryParameters
        {
            Page = 1, 
            PageSize = 10, 
            CategoryId = Category1
        });

        Assert.Equal(2, result.TotalCount);
        Assert.All(result.Items, p => Assert.Equal(Category1, p.CategoryId));
    }

    [Fact]
    public async Task GetProductsAsync_InvalidId_ReturnsNull()
    {
        await SeedAsync();
        var service = new ProductService(Db);

        var result = await service.GetProductAsync(Guid.NewGuid());

        Assert.Null(result);
    }
}
