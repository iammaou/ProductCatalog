using System;
using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.DTO;
using Service.Entities;

namespace Service.Services;

public class ProductCategoryService
{
    private readonly ApplicationDbContext dbContext;

    public ProductCategoryService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<ProductCategory>> GetAllCategoriesAsync()
    {
        return await dbContext.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategory?> GetCategoryAsync(Guid id)
    {
        return await dbContext.ProductCategories.FindAsync(id);
    }

    public async Task<ProductCategory> AddCategoryAsync(AddProductCategoryDTO addCategoryDTO)
    {
        var ProductCategoryEntity = new ProductCategory()
        {
            Name = addCategoryDTO.Name,
            Description = addCategoryDTO.Description
        };

        dbContext.ProductCategories.Add(ProductCategoryEntity);
        await dbContext.SaveChangesAsync();

        return ProductCategoryEntity;
    }
}
