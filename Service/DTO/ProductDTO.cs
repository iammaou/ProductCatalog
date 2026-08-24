using System;

namespace Service.DTO;

public class ProductDTO
{
    public required string Name {get;set;}
    public decimal Price {get;set;}
    public int StockQuantity {get;set;}
    public bool IsActive {get;set;}

    public Guid CategoryId {get;set;}
}
