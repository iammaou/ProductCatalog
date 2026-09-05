using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.DTO;
using Service.Entities;
using Service.Mappers;

namespace Service.Services;

public enum ProductCategoryDeleteResult
{
    NotFound,
    HasProducts,
    Success
}

public interface IProductCategoryService
{
    Task<PagedResult<ProductCategoryDTO>> GetAllCategoriesAsync(int page = 1, int pageSize = 10);
    Task<ProductCategoryDTO?> GetCategoryAsync(Guid id);
    Task<ProductCategoryDTO> AddCategoryAsync(ProductCategoryDTO productCategoryDTO);
    Task<ProductCategoryDeleteResult> RemoveCategoryAsync(Guid id);
    Task<ProductCategoryDTO?> UpdateCategoryAsync(Guid id, ProductCategoryDTO updateProductCategoryDTO);
}
public class ProductCategoryService(ApplicationDbContext dbContext) : IProductCategoryService
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<PagedResult<ProductCategoryDTO>> GetAllCategoriesAsync(int page = 1, int pageSize = 10)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var totalCount = await dbContext.ProductCategories.AsNoTracking().CountAsync();

        var categories = await dbContext.ProductCategories
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var categoriesDTO = categories.Select(c => ProductCategoryMappers.ToDTO(c)).ToList();

        return new PagedResult<ProductCategoryDTO>
        {
            Items = categoriesDTO,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ProductCategoryDTO?> GetCategoryAsync(Guid id)
    {
        var category = await dbContext.ProductCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if(category is null)
        {
            return null;
        }

        return ProductCategoryMappers.ToDTO(category);
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

        return ProductCategoryMappers.ToDTO(ProductCategoryEntity);
    }

    public async Task<ProductCategoryDeleteResult> RemoveCategoryAsync(Guid id)
    {
        var category = await dbContext.ProductCategories
            .Include(c => c.Products)  // Load related products
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if(category is null)
        {
            return ProductCategoryDeleteResult.NotFound;
        }

        if (category.Products.Count != 0)
        {
            return ProductCategoryDeleteResult.HasProducts;
        }

        dbContext.ProductCategories.Remove(category);
        await dbContext.SaveChangesAsync();
        return ProductCategoryDeleteResult.Success;
    }

    public async Task<ProductCategoryDTO?> UpdateCategoryAsync(Guid id, ProductCategoryDTO productCategoryDTO)
    {
        var productCategory = await dbContext.ProductCategories.FindAsync(id);

        if(productCategory is null)
        {
            return null;
        }
        
        productCategory.Name = productCategoryDTO.Name;
        productCategory.Description = productCategoryDTO.Description;

        await dbContext.SaveChangesAsync();

        return ProductCategoryMappers.ToDTO(productCategory);
    }
}
