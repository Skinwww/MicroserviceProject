using System.ComponentModel.DataAnnotations;

namespace OrderApi.Presentation.Application.DTOs
{
    public class OrderSummaryDTO
    {
        public int Id { get; set; }

        [Required, StringLength(100, ErrorMessage = "Имя не может превышать 100 символов.")]
        public string Name { get; set; }

        [Required, RegularExpression("^(0\\.5|1|2|5)$", ErrorMessage = "Объем должен быть 0.5 л, 1 л, 2 л или 5 л.")]
        public string Volume { get; set; }

        [Required, RegularExpression("^(негазированная|газированная|сладкий напиток|минеральная)$", ErrorMessage = "Тип должен быть 'негазированная', 'газированная', 'сладкий напиток' или 'минеральная'.")]
        public string Type { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Количество покупки должно быть положительным числом.")]
        public int PurchaseQuantity { get; set; }

        [Required, DataType(DataType.Currency), Range(0.01, double.MaxValue, ErrorMessage = "Цена за единицу должна быть положительной.")]
        public decimal UnitPrice { get; set; }

        [Required, DataType(DataType.Currency), Range(0.01, double.MaxValue, ErrorMessage = "Общая цена должна быть положительной.")]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime OrderedDate { get; set; }

        public OrderSummaryDTO(int id, string name, string volume, string type, int purchaseQuantity, decimal unitPrice, decimal totalPrice,
           DateTime orderedDate)
        {
            Id = id;
            Name = name;
            Volume = volume;
            Type = type;
            PurchaseQuantity = purchaseQuantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            OrderedDate = orderedDate;
        }
    }
}
