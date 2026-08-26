using System;

namespace Service.DTO;

public class ProductQueryParameters
{
    public int PageNumber {get;set;} = 1;
    public int PageSize {get;set;} = 10;

    public Guid? CategoryId {get;set;}
    public decimal? MinPrice {get;set;}
    public decimal? MaxPrice{get;set;}
    public bool? IsActive {get;set;}
    public int? StockQuantity {get;set;}

    public string? SortBy {get;set;} //price, name, createdat
    public bool IsDescending {get;set;} = false;
}
