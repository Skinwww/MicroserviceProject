using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            // возвращает 1 продукт
            if (product is not null) //проверяет что не null
            {
                var singleProduct = new ProductDTO(
                    product!.Id,
                    product.Name!,
                    product.Quantity,
                    product.Price,
                    product.Type,
                    product.Volume
                );

                return (singleProduct, null);
            }

            // возвращает список
            if (products is not null || product is null)
            {
                var _products = products.Select(p =>
                    new ProductDTO(
                        p.Id,
                        p.Name,
                        p.Quantity,
                        p.Price,
                        p.Type,
                        p.Volume
                    )).ToList();

                return (null, _products);
            }

            //если оба null
            return (null, null);
        }
    }
}

