using CouponApi.Application.DTO;
using CouponApi.Presentation.Domain.Entities;
using ECommercelib.SharedLibrary.Responses;

namespace CouponApi.Application.Interfaces
{
    public interface IPromoCode
    {
        Task<List<PromoCode>> GetUserPromoCodes(int userId);
        Task<List<PromoCode>> GetAvailablePromoCodes(int userId);
        Task<Response> AddPromoCode(PromoCodeDTO promoCodeDto);
        Task<Response> RedeemPromoCode(int promoCodeId, int userId); // Метод для использования промокода
    }
}
