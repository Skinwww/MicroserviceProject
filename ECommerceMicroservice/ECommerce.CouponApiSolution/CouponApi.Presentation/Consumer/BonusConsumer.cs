using CouponApi.Infrastructure.Data;
using CouponApi.Presentation.Domain.Entities;
using MassTransit;

namespace CouponApi.Presentation.Consumer
{
    public class BonusConsumer(CouponDbContext _context) : IConsumer<Bonus>
    {
        public async Task Consume(ConsumeContext<Bonus> context)
        {
            _context.Bonuses.Add(context.Message);
            await _context.SaveChangesAsync();
        }
    }
}
