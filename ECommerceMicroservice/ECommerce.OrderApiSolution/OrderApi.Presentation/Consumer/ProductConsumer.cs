using MassTransit;
using OrderApi.Infrastructure.Data;
using ProductApi.Infrastructure.Data;
using ProductApi.Presentation.Domain.Entities;


namespace OrderApi.Presentation.Consumer
{
    public class ProductConsumer(OrderDbContext _context) : IConsumer<Product>
    {
        public async Task Consume(ConsumeContext<Product> context)
        {
           _context.Products.Add(context.Message);
            await _context.SaveChangesAsync();
        }
    }
}
