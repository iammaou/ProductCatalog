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

    public async Task<List<ProductCategoryDTO>> GetAllCategoriesAsync()
    {
        var categories = await dbContext.ProductCategories.AsNoTracking().ToListAsync();

        var categoriesDTO = categories.Select(c => new ProductCategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();

        return categoriesDTO;
    }

    public async Task<ProductCategoryDTO?> GetCategoryAsync(Guid id)
    {
        var category = await dbContext.ProductCategories.FindAsync(id);

        if(category is null)
        {
            return null;
        }

        var categoriesDTO = new ProductCategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return categoriesDTO;
    }

    public async Task<ProductCategoryDTO> AddCategoryAsync(ProductCategoryDTO productCategoryDTO)
    {
        var ProductCategoryEntity = new ProductCategory()
        {
            Name = productCategoryDTO.Name,
            Description = productCategoryDTO.Description
        };

        dbContext.ProductCategories.Add(ProductCategoryEntity);
        await dbContext.SaveChangesAsync();

        var newProductCategoryDTO = new ProductCategoryDTO
        {
            Id = ProductCategoryEntity.Id,
            Name = ProductCategoryEntity.Name,
            Description = ProductCategoryEntity.Description
        };

        return newProductCategoryDTO;
    }

    public async Task<bool?> RemoveCategoryAsync(Guid id)
    {
        var category = await dbContext.ProductCategories.FindAsync(id);

        if(category is null || await dbContext.Products.AnyAsync(p => p.CategoryId == id))
        {
            return null;   
        }

        dbContext.ProductCategories.Remove(category);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<ProductCategoryDTO?> UpdateCategoryAsync(Guid id, ProductCategoryDTO productCategoryDTO)
    {
        var productCategory = await dbContext.ProductCategories.FindAsync(id);

        if(productCategory is null)
        {
            return null;
        }

        productCategory.Name = productCategoryDTO.Name ?? productCategory.Name;
        productCategory.Description = productCategoryDTO.Description ?? productCategory.Description;

        await dbContext.SaveChangesAsync();

        var newProductCategoryDTO = new ProductCategoryDTO
        {
            Id = productCategory.Id,
            Name = productCategory.Name,
            Description = productCategory.Description
        };

        return newProductCategoryDTO;
    }
}
