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
