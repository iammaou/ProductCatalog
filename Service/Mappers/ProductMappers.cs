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
}
