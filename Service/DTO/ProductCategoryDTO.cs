using System;

namespace Service.DTO;

public class ProductCategoryDTO
{
    public Guid Id {get; set;}
    public required string Name {get; set;}
    public required string Description {get;set;}
}
