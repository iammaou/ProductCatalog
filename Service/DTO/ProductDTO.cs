using System.ComponentModel.DataAnnotations;

namespace Service.DTO;

public class ProductDTO
{
    public Guid Id {get;set;}

    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 100 characters")]
    public required string Name {get;set;}

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price {get;set;}

    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    public int StockQuantity {get;set;}

    [Required(ErrorMessage = "Active status is required")]
    public bool IsActive {get;set;}
    public DateTime CreatedAt {get;set;}

    [Required(ErrorMessage = "Category ID is required")]
    public Guid CategoryId {get;set;}
}
