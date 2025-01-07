using System.ComponentModel.DataAnnotations;

namespace CouponApi.Application.DTO
{
    public class PromoCodeDTO
    {
        public string Code { get; private set; }
        public int BonusAmount { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? ExpirationDate { get; private set; }

        public PromoCodeDTO(string code, int bonusAmount, string description, DateTime createdDate, DateTime? expirationDate)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Код не должен быть пустой", nameof(code));
            if (bonusAmount <= 0)
                throw new ArgumentException("Бонусы болжны быть дольше 0", nameof(bonusAmount));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Описание не может быть пустым", nameof(description));

            Code = code;
            BonusAmount = bonusAmount;
            Description = description;
            CreatedDate = createdDate;
            ExpirationDate = expirationDate;
        }
    }
}
