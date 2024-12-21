using CouponApi.Presentation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CouponApi.Infrastructure.Data
{
    public class CouponDbContext: DbContext
    {
        public CouponDbContext(DbContextOptions<CouponDbContext> options) : base(options) { }

        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; } 
        public DbSet<BottleDeposit> BottleDeposits { get; set; }

    }
}
