using System.ComponentModel.DataAnnotations;

namespace Service.DTO;

public class ProductCategoryDTO
{
    public Guid Id {get; set;}

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 50 characters")]
    public required string Name {get; set;}

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public required string Description {get;set;}
}
