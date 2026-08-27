using System;

namespace Service.DTO;

public class ProductDTO
{
    public Guid Id {get;set;}
    public required string Name {get;set;}
    public decimal Price {get;set;}
    public int StockQuantity {get;set;}
    public bool IsActive {get;set;}
    public DateTime CreatedAt {get;set;}

    public Guid CategoryId {get;set;}
}
