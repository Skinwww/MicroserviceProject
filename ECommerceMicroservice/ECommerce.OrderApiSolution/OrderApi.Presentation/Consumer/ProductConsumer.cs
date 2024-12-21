using MassTransit;
using ProductApi.Infrastructure.Data;
using ProductApi.Presentation.Domain.Entities;


namespace OrderApi.Presentation.Consumer
{
    public class ProductConsumer(ProductDbContext _context) : IConsumer<Product>
    {
        public async Task Consume(ConsumeContext<Product> context)
        {
           _context.Products.Add(context.Message);
            await _context.SaveChangesAsync();
        }
    }
}
