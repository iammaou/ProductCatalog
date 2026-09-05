using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.DTO;
using Service.Entities;
using Service.Mappers;

namespace Service.Services;
public interface IProductService
{
    Task<PagedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParameters query);
    Task<ProductDTO?> GetProductAsync(Guid id);
    Task<ProductDTO> AddProductAsync(ProductDTO productDTO);
    Task<bool?> RemoveProductAsync(Guid id);
    Task<ProductDTO?> UpdateProductAsync(Guid id, ProductDTO productDTO);
}
public class ProductService(ApplicationDbContext dbContext) : IProductService
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<PagedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParameters query)
    {

        query.Validate();

        IQueryable<Product> productsQuery = dbContext.Products.AsNoTracking();

        if (query.CategoryId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.CategoryId == query.CategoryId.Value);
        }

        if (query.MinPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price <= query.MaxPrice.Value);
        }

        if (query.IsActive.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.IsActive == query.IsActive.Value);
        }

        if(query.IsInStock.HasValue && query.StockQuantity.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.StockQuantity == query.StockQuantity.Value);
        } 
        else if (query.IsInStock.HasValue)
        {
            if (query.IsInStock.Value)
            {
                productsQuery = productsQuery.Where(p => p.StockQuantity > 0);
            }
            else
            {
                productsQuery = productsQuery.Where(p => p.StockQuantity == 0);
            }
        }
        else if (query.StockQuantity.HasValue)
    {
        productsQuery = productsQuery.Where(p => p.StockQuantity == query.StockQuantity.Value);
    }

        productsQuery = query.SortBy?.ToLower() switch
        {
            "price" => query.IsDescending ? productsQuery.OrderByDescending(p => p.Price) : productsQuery.OrderBy(p => p.Price).ThenBy(p => p.Id),
            "name" => query.IsDescending ? productsQuery.OrderByDescending(p => p.Name) : productsQuery.OrderBy(p => p.Name).ThenBy(p => p.Id),
            "createdat" => query.IsDescending ? productsQuery.OrderByDescending(p => p.CreatedAt) : productsQuery.OrderBy(p => p.CreatedAt).ThenBy(p => p.Id),
            _ => productsQuery.OrderBy(p => p.Id)
        };

        var totalCount = await productsQuery.CountAsync();

        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var items = await productsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => ProductMappers.ToDTO(p))
            .ToListAsync();

        return new PagedResult<ProductDTO>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
    

    public async Task<ProductDTO?> GetProductAsync(Guid id)
    {
        var product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        if(product is null)
        {
            return null;
        }

        return ProductMappers.ToDTO(product);
    }

    public async Task<ProductDTO> AddProductAsync(ProductDTO productDTO)
    {
        if(!await dbContext.ProductCategories.AnyAsync(c => c.Id == productDTO.CategoryId))
        {
            throw new ArgumentException("Category does not exist");
        }

        var productEntity = new Product()
        {
            Name = productDTO.Name,
            Price = productDTO.Price,
            StockQuantity = productDTO.StockQuantity,
            IsActive = productDTO.IsActive,
            CreatedAt = DateTime.UtcNow,
            CategoryId = productDTO.CategoryId
        };

        dbContext.Products.Add(productEntity);
        await dbContext.SaveChangesAsync();

        return ProductMappers.ToDTO(productEntity);
    }

    public async Task<bool?> RemoveProductAsync(Guid id) 
    {
        var product = await dbContext.Products.FindAsync(id);

        if(product is null)
        {
            return null;   
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<ProductDTO?> UpdateProductAsync(Guid id, ProductDTO productDTO)
    {
        if(!await dbContext.ProductCategories.AnyAsync(c => c.Id == productDTO.CategoryId))
        {
            throw new ArgumentException("Category does not exist");
        }

        var product = await dbContext.Products.FindAsync(id);

        if(product is null)
        {
            return null;
        }

        if(productDTO.Name is null)
        {
            throw new ArgumentException("Name is required when updating");
        }

        product.Name = productDTO.Name;
        product.Price = productDTO.Price;
        product.StockQuantity = productDTO.StockQuantity;
        product.IsActive = productDTO.IsActive;
        product.CategoryId = productDTO.CategoryId;

        await dbContext.SaveChangesAsync();
        
        return ProductMappers.ToDTO(product);
    }
}
