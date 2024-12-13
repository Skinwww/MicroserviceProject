﻿using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
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

