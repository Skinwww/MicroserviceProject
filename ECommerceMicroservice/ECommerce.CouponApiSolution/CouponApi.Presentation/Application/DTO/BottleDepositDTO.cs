namespace CouponApi.Application.DTO
{
    public class BottleDepositDTO
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int Count { get; private set; }
        public DateTime DateDeposited { get; private set; }

        public BottleDepositDTO(int id, int userId, int count, DateTime dateDeposited)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId должен быть больше 0", nameof(userId));
            if (count < 0)
                throw new ArgumentException("Число дожно быть больше 0", nameof(count));

            Id = id;
            UserId = userId;
            Count = count;
            DateDeposited = dateDeposited;
        }
    }
}
