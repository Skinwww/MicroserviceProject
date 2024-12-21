using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApi.Application.DTO
{
    public record BottleDepositDTO
    (
    int Id,
    int UserId,
    int Count,
    DateTime DateDeposited
    );
}
