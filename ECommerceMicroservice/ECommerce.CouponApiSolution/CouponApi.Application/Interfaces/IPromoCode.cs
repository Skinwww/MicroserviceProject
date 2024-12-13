using CouponApi.Application.DTO;
using CouponApi.Domain.Entities;
using ECommercelib.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
