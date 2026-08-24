using System;
using System.ComponentModel;
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

    public async Task<ProductCategory> AddCategoryAsync(ProductCategoryDTO productCategoryDTO)
    {
        var ProductCategoryEntity = new ProductCategory()
        {
            Name = productCategoryDTO.Name,
            Description = productCategoryDTO.Description
        };

        dbContext.ProductCategories.Add(ProductCategoryEntity);
        await dbContext.SaveChangesAsync();

        return ProductCategoryEntity;
    }

    public async Task<ProductCategory?> RemoveCategoryAsync(Guid id)
    {
        var category = await dbContext.ProductCategories.FindAsync(id);

        if(category is null)
        {
            return null;   
        }

        dbContext.ProductCategories.Remove(category);
        await dbContext.SaveChangesAsync();

        return category;
    }

    public async Task<ProductCategory?> UpdateCategoryAsync(Guid id, ProductCategoryDTO productCategoryDTO)
    {
        var productCategory = await dbContext.ProductCategories.FindAsync(id);

        if(productCategory is null)
        {
            return null;
        }

        productCategory.Name = productCategoryDTO.Name ?? productCategory.Name;
        productCategory.Description = productCategoryDTO.Description ?? productCategory.Description;

        await dbContext.SaveChangesAsync();

        return productCategory;
    }
}
