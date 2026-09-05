using Service.DTO;
using Service.Entities;

namespace Service.Mappers;

public static class ProductCategoryMappers
{
    public static ProductCategoryDTO ToDTO(this ProductCategory productCategory)
    {
        ArgumentNullException.ThrowIfNull(productCategory);

        return new ProductCategoryDTO
        {
            Id = productCategory.Id,
            Name = productCategory.Name,
            Description = productCategory.Description
        };
    }
}
