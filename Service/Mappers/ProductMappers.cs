using System;
using Service.DTO;
using Service.Entities;

namespace Service.Mappers;

public static class ProductMappers
{
    public static ProductDTO ToDTO(this Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            CategoryId = product.CategoryId
        };
    }

    public static Product ToEntity(this ProductDTO productDto)
    {
        ArgumentNullException.ThrowIfNull(productDto);

        return new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            StockQuantity = productDto.StockQuantity,
            IsActive = productDto.IsActive,
            CreatedAt = DateTime.UtcNow,
            CategoryId = productDto.CategoryId
        };
    }
}
