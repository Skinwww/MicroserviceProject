using ProductApi.Presentation.Domain.Entities;

namespace ProductApi.Application.DTOs.Conversions
{
    public class ProductConversion
    {
        public static Product ToEntity(ProductDTO productDto) => new()
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Quantity = productDto.Quantity,
            Price = productDto.Price,
            Type = productDto.Type,
            Volume = productDto.Volume
           
        };

        public static IEnumerable<ProductDTO> FromEntity(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
            {
                return Enumerable.Empty<ProductDTO>(); // Возвращаем пустой список, если входной список null или пуст
            }

            return products.Select(o => new ProductDTO(
                o.Id,
                o.Name,
                o.Quantity,
                o.Price,
                o.Type,
                o.Volume
            ));
        }
    }
    }


