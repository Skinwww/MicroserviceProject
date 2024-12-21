namespace CouponApi.Presentation.Application.DTO
{
    public class BonusSummaryDTO
    {
        
        public int Amount { get; private set; }

        public BonusSummaryDTO(int amount)
        {
            if (amount <= 0) throw new ArgumentException("Число должно быть больше 0", nameof(amount));

            Amount = amount;           
        }
    }
}
