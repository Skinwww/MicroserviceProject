using CouponApi.Domain.Entities;


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

        public static (BonusDTO? singleBonus, IEnumerable<BonusDTO> allBonuses) FromEntity(Bonus? bonus, IEnumerable<Bonus>? bonuses)
        {

            if (bonus is not null)
            {
                var singleBonus = new BonusDTO
                (
                    bonus.Id,
                    bonus.UserId,
                    bonus.Amount,
                    bonus.EarnedDate,
                    bonus.IsRedeemed = false
                );

                return (singleBonus, Enumerable.Empty<BonusDTO>());
            }


            if (bonuses is not null)
            {
                var allBonusesDTO = bonuses.Select(o =>
                    new BonusDTO(o.Id,
                                 o.UserId,
                                 o.Amount,
                                 o.EarnedDate,
                                 o.IsRedeemed = false
                    ));

                return (null, allBonusesDTO);
            }

            return (null, Enumerable.Empty<BonusDTO>());
        }
    }
}
