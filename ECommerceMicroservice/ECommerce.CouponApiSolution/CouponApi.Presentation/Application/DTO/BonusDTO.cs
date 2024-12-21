using System.ComponentModel.DataAnnotations;

namespace CouponApi.Application.DTO
{
    public record BonusDTO(
    int Id,
    [Required] int UserId,
    [Required] int Amount,
    [Required] DateTime EarnedDate,
    [Required] bool IsRedeemed
    );
}
