using System.ComponentModel.DataAnnotations;

namespace CouponApi.Application.DTO
{
    public class BonusDTO
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int Amount { get; private set; }
        public DateTime EarnedDate { get; private set; }
        public bool IsRedeemed { get; private set; }

        public BonusDTO(int id, int userId, int amount, DateTime earnedDate, bool isRedeemed)
        {
            if (userId <= 0) throw new ArgumentException("UserId должен быть больше 0.", nameof(userId));
            if (amount <= 0) throw new ArgumentException("Число должно быть больше 0", nameof(amount));

            Id = id;
            UserId = userId;
            Amount = amount;
            EarnedDate = earnedDate;
            IsRedeemed = isRedeemed;
        }
    }
}
