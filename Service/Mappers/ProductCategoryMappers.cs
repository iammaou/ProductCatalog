using System;
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

    public static ProductCategory ToEntity(this ProductCategoryDTO productCategoryDTO)
    {
        ArgumentNullException.ThrowIfNull(productCategoryDTO);

        return new ProductCategory
        {
            Id = productCategoryDTO.Id,
            Name = productCategoryDTO.Name,
            Description = productCategoryDTO.Description
        };
    }

    public static ProductCategoryDTO UpdateToDTO(this UpdateProductCategoryDTO updateProductCategoryDTO)
    {
        ArgumentNullException.ThrowIfNull(updateProductCategoryDTO);

        return new ProductCategoryDTO
        {
            Name = updateProductCategoryDTO.Name,
            Description = updateProductCategoryDTO.Description
        };
    }
}
