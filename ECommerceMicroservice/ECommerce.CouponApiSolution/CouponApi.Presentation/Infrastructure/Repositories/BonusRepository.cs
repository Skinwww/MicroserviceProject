using CouponApi.Application.DTO;
using CouponApi.Application.DTO.Conversions;
using CouponApi.Application.Interfaces;
using CouponApi.Presentation.Domain.Entities;
using CouponApi.Infrastructure.Data;
using ECommercelib.SharedLibrary.Logs;
using ECommercelib.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;


namespace CouponApi.Infrastructure.Repositories
{
    public class BonusRepository(CouponDbContext context) : IBonus
    {
        public async Task<Response> AddBonus(int userId, BonusDTO bonusDTO)
        {
            try
            {
                var bonus = BonusConversions.ToEntity(bonusDTO);
                bonus.UserId = userId;

                await context.Bonuses.AddAsync(bonus);
                await context.SaveChangesAsync();
                return new Response(true, "Бонусы успешно зачислены");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Возникла ошибка при зачислении бонусов");
            }
        }

        public async Task<IEnumerable<BonusDTO>> GetBonusesByUserId(int userId)
        {
            try
            {
                var bonuses = await context.Bonuses
                    .Where(b => b.UserId == userId)
                    .ToListAsync();

                var (_, bonusDTOs) = BonusConversions.FromEntity(null, bonuses); //только бонусы
                return bonusDTOs!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Возникла ошибка при получении бонусов");
            }
        }

        public async Task<int> GetUserTotalBonus(int userId)
        {
            try
            {
                return await context.Bonuses
                      .Where(b => b.UserId == userId && !b.IsRedeemed)
                      .SumAsync(b => b.Amount);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Возникла ошибка при получении бонусов");
            }
        }

        public async Task<List<PromoCode>> RedeemBonus(int userId)
        {
            return await context.PromoCodes
        .Where(pc => !pc.IsUsed && pc.UserId == userId)
        .ToListAsync();
        }

        public async Task<Response> ReturnBottle(int userId, BottleDepositDTO bottleDepositDTO)
        {
            try
            {
                var bottleDeposit = new BottleDeposit
                {
                    UserId = userId,
                    Count = bottleDepositDTO.Count,
                    DateDeposited = DateTime.UtcNow
                };

                await context.BottleDeposits.AddAsync(bottleDeposit);
                await context.SaveChangesAsync();

                return new Response(true, "Бутылки успешно сданы");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Возникла ошибка при возврате бутылок");
            }
        }

        public async Task<List<BottleDeposit>> GetUserBottleDeposits(int userId)
        {
            return await context.BottleDeposits
                .Where(bd => bd.UserId == userId)
                .ToListAsync();
        }

        public async Task<Response> ReduceBonuses(int userId, int amount)
        {
            try
            {
                var bonuses = await context.Bonuses
                    .Where(b => b.UserId == userId && !b.IsRedeemed)
                    .ToListAsync();

                int totalAvailableBonus = bonuses.Sum(b => b.Amount);

                if (totalAvailableBonus < amount)
                {
                    return new Response(false, "Недостаточно бонусов для обмена");
                }

                while (amount > 0 && bonuses.Count > 0)
                {
                    var bonusToRedeem = bonuses.First();

                    if (bonusToRedeem.Amount <= amount)
                    {
                        amount -= bonusToRedeem.Amount;
                        bonusToRedeem.IsRedeemed = true;
                    }
                    else
                    {
                        bonusToRedeem.Amount -= amount;
                        amount = 0; 
                    }

                    context.Bonuses.Update(bonusToRedeem); 
                    bonuses.Remove(bonusToRedeem);
                }

                await context.SaveChangesAsync();
                return new Response(true, "Бонусы успешно списаны");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Возникла ошибка при списании бонусов");
            }
        }

    }
}
