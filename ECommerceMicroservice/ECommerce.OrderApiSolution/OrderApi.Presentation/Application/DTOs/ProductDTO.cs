using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required, StringLength(100, ErrorMessage = "Имя продукта не может превышать 100 символов.")]
        public required string Name { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Количество должно быть положительным числом.")]
        public required int Quantity { get; set; }
        [Required, DataType(DataType.Currency), Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть положительной.")]
        public required decimal Price { get; set; }
        [Required, RegularExpression("^(негазированная|газированная|сладкий напиток|минеральная)$", ErrorMessage = "Тип должен быть 'негазированная', 'газированная', 'сладкий напиток' или 'минеральная'.")]
        public required string Type { get; set; }
        [Required, RegularExpression("^(0\\.5|1|2|5)$", ErrorMessage = "Объем должен быть 0.5 л, 1 л, 2 л или 5 л.")]
        public required string Volume { get; set; }

        public ProductDTO(int id, string name, int quantity, decimal price, string type, string volume)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Price = price;
            Type = type;
            Volume = volume;
        }
    }
    }
