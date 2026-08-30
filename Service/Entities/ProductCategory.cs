using System;

namespace Service.Entities;

public class ProductCategory
{
    public Guid Id {get; set;}
    public required string Name {get; set;}
    public required string Description {get;set;}

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
