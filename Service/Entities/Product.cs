using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Entities;

public class Product
{
    public Guid Id {get;set;}
    public required string Name {get;set;}
    [Column(TypeName = "decimal(18,2)")]public decimal Price {get;set;}
    public int StockQuantity {get;set;}
    public bool IsActive {get;set;}
    public DateTime CreatedAt {get;set;}

    public Guid CategoryId {get;set;}
    public ProductCategory Category {get;set;} = null!;

    public byte[] RowVersion {get;set;} = [];
}
