using System;

namespace Service.DTO;

public class AddProductCategoryDTO
{
    public Guid Id {get; set;}
    public required string Name {get; set;}
    public required string Description {get;set;}
}
