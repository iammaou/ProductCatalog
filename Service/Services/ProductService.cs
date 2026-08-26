using System;
using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.DTO;
using Service.Entities;

namespace Service.Services;
 public interface IProductService
{
    Task<PagedResult<ProductDTO>> GetProductsAsync(ProductQueryParameters query);
}
public class ProductService : IProductService
{
    private readonly ApplicationDbContext dbContext;


    public ProductService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<PagedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParameters query)
    {
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

        if (query.StockQuantity.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.StockQuantity == query.StockQuantity.Value);
        }

        productsQuery = query.SortBy?.ToLower() switch
        {
            "price" => query.IsDescending ? productsQuery.OrderByDescending(p => p.Price) : productsQuery.OrderBy(p => p.Price),
            "name" => query.IsDescending ? productsQuery.OrderByDescending(p => p.Name) : productsQuery.OrderBy(p => p.Name),
            "createdat" => query.IsDescending ? productsQuery.OrderByDescending(p => p.CreatedAt) : productsQuery.OrderBy(p => p.CreatedAt),
            _ => productsQuery.OrderBy(p => p.Id)
        };

        var TotalCount = await productsQuery.CountAsync();

        var items = await productsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new ProductDTO
            {
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId
            })
            .ToListAsync();

        return new PagedResult<ProductDTO>
        {
            Items = items,
            TotalCount = TotalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
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

    public Task<PagedResult<ProductDTO>> GetProductsAsync(ProductQueryParameters query)
    {
        throw new NotImplementedException();
    }
}
