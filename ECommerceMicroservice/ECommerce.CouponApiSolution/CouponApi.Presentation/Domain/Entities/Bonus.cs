namespace CouponApi.Presentation.Domain.Entities
{
    public class Bonus
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public DateTime EarnedDate { get; set; }
        public bool IsRedeemed { get; set; } = false;
    }
}
