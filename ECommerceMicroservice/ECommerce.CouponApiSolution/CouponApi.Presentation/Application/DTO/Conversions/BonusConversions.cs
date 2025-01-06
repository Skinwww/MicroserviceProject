using CouponApi.Presentation.Domain.Entities;


namespace CouponApi.Application.DTO.Conversions
{
    public class BonusConversions
    {
        public static Bonus ToEntity(BonusDTO bonus) => new()
        {
            Id = bonus.Id,
            UserId = bonus.UserId,
            Amount = bonus.Amount,
            EarnedDate = bonus.EarnedDate,
            IsRedeemed = false
        };

        public static IEnumerable<BonusDTO> FromEntity(IEnumerable<Bonus> bonuses)
        {
            if (bonuses is null)
            {
                return Enumerable.Empty<BonusDTO>(); // Возвращаем пустой список, если входной список null
            }

            return bonuses.Select(o => new BonusDTO(
                o.Id,
                o.UserId,
                o.Amount,
                o.EarnedDate,
                o.IsRedeemed
            ));
        }
    }
}
