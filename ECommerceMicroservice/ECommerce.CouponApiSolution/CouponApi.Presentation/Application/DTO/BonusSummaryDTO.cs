namespace CouponApi.Presentation.Application.DTO
{
    public class BonusSummaryDTO
    {
        
        public int Amount { get; set; }
        public int UserTotalBonus { get; set; }

        public BonusSummaryDTO(int amount, int userTotalBonus)
        {
            if (amount <= 0) throw new ArgumentException("Число должно быть больше 0", nameof(amount));

            Amount = amount;           
            UserTotalBonus = userTotalBonus;
        }
    }
}
