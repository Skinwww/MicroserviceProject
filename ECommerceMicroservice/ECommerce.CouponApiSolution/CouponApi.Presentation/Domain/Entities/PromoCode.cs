namespace CouponApi.Presentation.Domain.Entities
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int BonusAmount { get; set; } 
        public string? Description { get; set; } 
        public int UserId { get; set; } 
        public DateTime CreatedDate { get; set; }
        public bool IsUsed { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
