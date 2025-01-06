using System.ComponentModel.DataAnnotations;

namespace ProductApi.Presentation.Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string? Type { get; set; }

        public string? Volume { get; set; }

    }
}
