using System;
using System.ComponentModel.DataAnnotations;

namespace Service.DTO;

public class PagedResult<T>
{
    public IEnumerable<T> Items {get;set;} = [];
    public int TotalCount {get;set;}
    [Range(1, int.MaxValue)] public int PageNumber {get;set;}
    [Range(1, 100)] public int PageSize {get;set;}
}
