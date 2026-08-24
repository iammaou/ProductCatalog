using System;
using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.DTO;
using Service.Entities;

namespace Service.Services;

public class ProductService
{
    private readonly ApplicationDbContext dbContext;

    public ProductService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetProductAsync(Guid id)
    {
        return await dbContext.Products.FindAsync(id);
    }

    public async Task<Product> AddCategoryAsync(ProductDTO productDTO)
    {
        var ProductEntity = new Product()
        {
            Name = productDTO.Name,
            Price = productDTO.Price,
            StockQuantity = productDTO.StockQuantity,
            IsActive = productDTO.IsActive,
            CategoryId = productDTO.CategoryId
        };

        dbContext.Products.Add(ProductEntity);
        await dbContext.SaveChangesAsync();

        return ProductEntity;
    }

    public async Task<Product?> RemoveProductAsync(Guid id)
    {
        var product = await dbContext.Products.FindAsync(id);

        if(product is null)
        {
            return null;   
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateCategoryAsync(Guid id, UpdateProductDTO productDTO)
    {
        var product = await dbContext.Products.FindAsync(id);

        if(product is null)
        {
            return null;
        }

        product.Name = productDTO.Name ?? product.Name;
        product.Price = productDTO.Price ?? product.Price;
        product.StockQuantity = productDTO.StockQuantity ?? product.StockQuantity;
        product.IsActive = productDTO.IsActive ?? product.IsActive;
        product.CategoryId = productDTO.CategoryId ?? product.CategoryId;

        await dbContext.SaveChangesAsync();

        return product;
    }
}
