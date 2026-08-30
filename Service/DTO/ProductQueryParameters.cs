using System;
using System.ComponentModel.DataAnnotations;

namespace Service.DTO;

public class ProductQueryParameters
{
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
    public int PageNumber {get;set;} = 1;

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize {get;set;} = 10;

    public Guid? CategoryId {get;set;}

    [Range(0, double.MaxValue, ErrorMessage = "Min price must be greater than or equal to 0")]
    public decimal? MinPrice {get;set;}

    [Range(0, double.MaxValue, ErrorMessage = "Max price must be greater than or equal to 0")]
    public decimal? MaxPrice{get;set;}
    public bool? IsActive {get;set;}

     [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be greater than or equal to 0")]
    public int? StockQuantity {get;set;}

    private string? _sortBy;
    public string? SortBy
    {
        get => _sortBy;
        set
        {
            // Only allow specific values
            var allowedValues = new[] { "price", "name", "createdat", null };
            if (value != null && !allowedValues.Contains(value.ToLower()))
            {
                throw new ArgumentException($"SortBy must be one of: {string.Join(", ", allowedValues)}");
            }
            _sortBy = value;
        }
    }
    public bool IsDescending {get;set;} = false;

    public void Validate()
    {
        if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
        {
            throw new ArgumentException("MinPrice cannot be greater than MaxPrice");
        }
    }
}
