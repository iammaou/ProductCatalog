using System;
using System.ComponentModel.DataAnnotations;

namespace Service.DTO;

public class UpdateProductDTO
{

    [StringLength(100, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 100 characters")]
    public string? Name {get;set;}

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal? Price {get;set;}

    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    public int? StockQuantity {get;set;}
    public bool? IsActive {get;set;}

    public Guid? CategoryId {get;set;}
}
