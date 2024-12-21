using OrderApi.Presentation.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order) => new()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            OrderDate = order.OrderDate,
            PurchaseQuantity = order.PurchaseQuantity,
            Status = order.Status
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)

        {
            //return single
            if(order is not  null || orders is null)
            {
                var singleOrder = new OrderDTO
                    (order!.Id, 
                    order.ClientId, 
                    order.ProductId, 
                    order.PurchaseQuantity, 
                    order.OrderDate,
                    order.Status);

                return (singleOrder, null);
            }

            //return list
            if(order is not null || order is null)
            {
                var _orders = orders!.Select(o =>
                new OrderDTO(o.Id,
                o.ClientId,
                o.ProductId,
                o.PurchaseQuantity,
                o.OrderDate,
                o.Status
                ));
                return (null, _orders);
            }
            return (null, null);
        }

    }
}
