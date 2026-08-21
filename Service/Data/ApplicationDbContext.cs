using System;
using Microsoft.EntityFrameworkCore;
using Service.Entities;

namespace Service.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products {get; set;}
    public DbSet<ProductCategory> ProductCategories {get;set;}
}
