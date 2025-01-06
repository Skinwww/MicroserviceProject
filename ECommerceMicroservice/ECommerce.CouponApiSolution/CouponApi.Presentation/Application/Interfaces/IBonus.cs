using CouponApi.Presentation.Domain.Entities;
using CouponApi.Application.DTO;
using ECommercelib.SharedLibrary.Responses;
using CouponApi.Presentation.Application.DTO;

namespace CouponApi.Application.Interfaces
{
    public interface IBonus
    {
        Task<Response> AddBonus(int userId, BonusDTO bonusDTO);
        Task<List<PromoCode>> RedeemBonus(int userId);
        Task<IEnumerable<BonusDTO>> GetBonusesByUserId(int userId);
        Task<int> GetUserTotalBonus(int userId);
        Task<Response> ReduceBonuses(int userId, int amount);
        Task<BonusSummaryDTO> GetOrderSummaryAsync(int bonusId);

        //для работы с BottleDeposit
        Task<Response> ReturnBottle(int userId, BottleDepositDTO bottleDepositDTO);
        Task<List<BottleDeposit>> GetUserBottleDeposits(int userId);
    }
}
