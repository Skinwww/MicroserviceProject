using CouponApi.Application.DTO;
using CouponApi.Application.Interfaces;
using CouponApi.Domain.Entities;
using CouponApi.Infrastructure.Data;
using ECommercelib.SharedLibrary.Logs;
using ECommercelib.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;

namespace CouponApi.Infrastructure.Repositories
{
    public class PromoCodeRepository: IPromoCode
    {
        private readonly CouponDbContext context;
        private readonly BonusRepository bonusRepository;

        public PromoCodeRepository(CouponDbContext context)
        {
            this.context = context;
            this.bonusRepository = new BonusRepository(context);
        }


        public async Task<List<PromoCode>> GetUserPromoCodes(int userId)
        {
            return await context.PromoCodes
        .Where(pc => pc.UserId == userId && pc.IsUsed) 
        .ToListAsync();
        }

        public async Task<List<PromoCode>> GetAvailablePromoCodes(int userId)
        {
            return await context.PromoCodes
                .Where(pc => !pc.IsUsed)
                .ToListAsync();
        }
        public async Task<Response> AddPromoCode(PromoCodeDTO promoCodeDTO)
        {

            try
            {
                var promoCode = new PromoCode
                {
                    Code = promoCodeDTO.Code,
                    BonusAmount = promoCodeDTO.BonusAmount,
                    Description = promoCodeDTO.Description,
                    CreatedDate = promoCodeDTO.CreatedDate,
                    ExpirationDate = promoCodeDTO.ExpirationDate,
                    IsUsed = false
                };

                await context.PromoCodes.AddAsync(promoCode);
                await context.SaveChangesAsync();

                return new Response(true, "Промокод успешно добавлен");
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование или обработку ошибок
                LogException.LogExceptions(ex);
                return new Response(false, "Возникла ошибка при добавлении промокода");
            }
        }

        public async Task<Response> RedeemPromoCode(int promoCodeId, int userId)
        {
            try
            {
                var promoCode = await context.PromoCodes.FindAsync(promoCodeId);
                if (promoCode == null || promoCode.IsUsed)
                {
                    return new Response(false, "Промокод не найден или уже использован");
                }

                // Используем метод из BonusRepository
                var totalBonus = await bonusRepository.GetUserTotalBonus(userId);
                if (totalBonus < promoCode.BonusAmount)
                {
                    return new Response(false, "Недостаточно бонусов для списания");
                }

                promoCode.UserId = userId;
                promoCode.IsUsed = true;

                var response = await bonusRepository.ReduceBonuses(userId, promoCode.BonusAmount);
                if (!response.Flag)
                {
                    return response;
                }

                await context.SaveChangesAsync();
                return new Response(true, "Промокод успешно получен");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Возникла ошибка при получении промокода");
            }
        }
    }
}
    
