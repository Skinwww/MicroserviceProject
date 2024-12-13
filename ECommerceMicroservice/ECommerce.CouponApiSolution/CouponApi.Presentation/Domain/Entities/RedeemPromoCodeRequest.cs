namespace CouponApi.Presentation.Domain.Entities
{
    public class RedeemPromoCodeRequest
    {
        public int PromoCodeId { get; set; }
        public int BonusAmount { get; set; }
        public int UserId { get; set; }
    }
}
