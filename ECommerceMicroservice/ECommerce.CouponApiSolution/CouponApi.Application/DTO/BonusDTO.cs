using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
