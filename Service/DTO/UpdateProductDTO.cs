using System;

namespace Service.DTO;

public class UpdateProductDTO
{
    public string? Name {get;set;}
    public decimal? Price {get;set;}
    public int? StockQuantity {get;set;}
    public bool? IsActive {get;set;}

    public Guid? CategoryId {get;set;}
}
