using OrderApi.Presentation.Application.DTOs;
using OrderApi.Presentation.Domain.Entities;
using ProductApi.Presentation.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {

        public static Order ToEntity(OrderDTO order) => new()
        {
            Id = order.Id,
            ProductId = order.ProductId,
            ClientId = order.ClientId,
            PurchaseQuantity = order.PurchaseQuantity,
            OrderDate = order.OrderDate,
            Status = order.Status
        };


        public static IEnumerable<OrderDTO> FromEntity(IEnumerable<Order> orders)
        {
            if (orders is null)
            {
                return Enumerable.Empty<OrderDTO>(); // Возвращаем пустой список, если входной список null
            }

            return orders.Select(o => new OrderDTO(
                o.Id,
                o.ProductId,
                o.ClientId,
                o.PurchaseQuantity,
                o.OrderDate,
                o.Status
            ));
        }
    }
}
