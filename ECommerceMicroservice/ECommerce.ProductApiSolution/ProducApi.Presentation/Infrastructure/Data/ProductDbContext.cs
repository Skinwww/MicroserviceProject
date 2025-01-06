using Microsoft.EntityFrameworkCore;
using ProductApi.Presentation.Domain.Entities;
namespace ProductApi.Infrastructure.Data
{
    public class ProductDbContext : DbContext 
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) 
        {
        }

        public DbSet<Product> Products { get; set; }
       
    }
}


