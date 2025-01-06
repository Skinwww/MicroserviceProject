using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public class OrderDTO
    {
        public OrderDTO(int id,int productId,int clientId, int purchaseQuantity, DateTime orderDate, string status)
        {
            Id = id;
            ProductId= productId;
            ClientId = clientId;
            PurchaseQuantity = purchaseQuantity;
            OrderDate = orderDate;
            Status = status;
        }

        public int Id { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "ProductId должен быть положительным числом.")]
        public int ProductId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "ClientId должен быть положительным числом.")]
        public int ClientId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Количество покупок должно быть положительным числом.")]
        public int PurchaseQuantity { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required, RegularExpression("^(новый|в обработке|собран)$", ErrorMessage = "Статус может быть 'новый', 'в обработке' или 'собран'.")]
        public string Status { get; set; }
    }
};

