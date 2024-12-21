using System.ComponentModel.DataAnnotations;

namespace CouponApi.Application.DTO
{
    public record PromoCodeDTO
    (
  string Code ,
    [Required] int BonusAmount,
  [Required] string Description,
    [Required] DateTime CreatedDate ,
   [Required] DateTime? ExpirationDate);
}
