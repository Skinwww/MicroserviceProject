namespace CouponApi.Presentation.Domain.Entities
{
    public class BottleDeposit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Count { get; set; }
        public DateTime DateDeposited { get; set; }
    }
}
