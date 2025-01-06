using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
