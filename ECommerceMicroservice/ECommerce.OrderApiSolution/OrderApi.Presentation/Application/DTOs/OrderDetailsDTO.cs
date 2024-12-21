using OrderApi.Presentation.Domain.Entities;
using ProductApi.Presentation.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Xml.Linq;

namespace OrderApi.Application.DTOs
{
    public class OrderDetailsDTO
    {
        public int OrderId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "ProductId должен быть положительным числом.")]
        public int ProductId { get; set; }
        [Required]
        public int Client { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required, RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Номер телефона должен содержать от 10 до 15 цифр и может начинаться с +.")]
        public string TelephoneNumber { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required, RegularExpression("^(0\\.5|1|2|5)$", ErrorMessage = "Объем может быть 0.5 л, 1 л, 2 л или 5 л.")]
        public string Volume { get; set; }
        [Required, RegularExpression("^(негазированная|газированная|сладкий напиток|минеральная)$", ErrorMessage = "Тип должен быть 'негазированная', 'газированная', 'сладкий напиток' или 'минеральная'.")]
        public string Type { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Количество покупок не может быть отрицательным.")]
        public int PurchaseQuantity { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена за единицу должна быть положительной.")]
        public decimal UnitPrice { get; set; }
        [Required, DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Общая цена должна быть положительной.")]
        public decimal TotalPrice { get; set; }
        [Required]
        public DateTime OrderedDate { get; set; }
        [Required, RegularExpression("^(новый|в обработке|собран)$", ErrorMessage = "Статус может быть 'новый', 'в обработке' или 'собран'.")]
        public string Status { get; set; }

        public OrderDetailsDTO(int orderId, int productId, int clientId, string name, string email, string address,
            string telephoneNumber, string productName, string volume, string type,int purchaseQuantity, decimal unitPrice, decimal totalPrice,
            DateTime orderedDate, string status)
        {
            OrderId = orderId;
            ProductId = productId;
            Client = clientId;
            Name = name;
            Email = email;
            Address = address;
            TelephoneNumber = telephoneNumber;
            ProductName = productName;
            Volume = volume;
            Type = type;
            PurchaseQuantity = purchaseQuantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            OrderedDate = orderedDate;
            Status = status;
        }
    };
}
