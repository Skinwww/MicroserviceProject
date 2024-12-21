
using ECommercelib.SharedLibrary.DTOs;
using ECommercelib.SharedLibrary.Logs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Presentation.Application.DTOs;
using OrderApi.Presentation.Domain.Entities;
using System.Linq.Expressions;
using System.Text;
using Response = ECommercelib.SharedLibrary.Responses.Response;


namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context, IPublishEndpoint publishEndpoint) : IOrderRepository
    {
        
        public async Task<Response> CreateAsync(Order _order)
        {
            try
            {
                context.Orders.Add(_order);
                await context.SaveChangesAsync();
                var orderSummary = await GetOrderSummaryAsync(_order.Id);
                if (orderSummary == null)
                {
                    return new Response(false, "Данные о заказе не были найдены");
                }
                string content = BuildOrderEmailBody(orderSummary.Id, orderSummary.Name, orderSummary.Volume, orderSummary.Type, 
                    orderSummary.PurchaseQuantity, orderSummary.UnitPrice, orderSummary.TotalPrice, orderSummary.OrderedDate);
                await publishEndpoint.Publish(new EmailDTO("Информация о заказе", content));
                await ClearOrderTable();
                return new Response(true, "Заказ успешно оформлен");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Появилась ошибка при оформлении заказа");
            }
        }

        public async Task<Response> DeleteAsync(Order _order)
        {
            try
            {
                var order = await FindByIdAsync(_order.Id);
                if (order is null)
                    return new Response(false, "Заказ не найден");

                context.Orders.Remove(_order);
                await context.SaveChangesAsync();
                return new Response(true, "Заказ удален");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Возникла ошибка при удалении заказа");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders.FindAsync(id);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                throw new Exception("Возникла ошибка при получении заказа");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                throw new Exception("Возникла ошибка при получении заказа");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

              throw new Exception("Возникла ошибка при получении заказа");
            }

        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Возникла ошибка при получении заказа");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order is null)
                    return new Response(false, $"Заказ не найден");

                context.Entry(order).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Заказ обновлен");
               
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                return new Response(false, "возникла ошибка при размещении заказа");
            }
        }

        public async Task<Response> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                var order = await FindByIdAsync(orderId);
                if (order is null)
                    return new Response(false, "Заказ не найден");

                order.Status = newStatus; 
                context.Orders.Update(order);
                await context.SaveChangesAsync();

                return new Response(true, "Статус заказа изменен");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Возникла ошибка при обновлении статуса заказа");
            }
        }

        public async Task<OrderSummaryDTO> GetOrderSummaryAsync(int orderId)
        {
            var order = await context.Orders.FindAsync(orderId);
            if (order == null) return null;
            var products = await context.Products.ToListAsync();
            var productInfo = products.Find(x => x.Id == order!.ProductId);
            if (productInfo == null) return null;

            return new OrderSummaryDTO(
                order!.Id,
                productInfo.Name,
                productInfo!.Volume,
                productInfo.Type,
                order.PurchaseQuantity,
                productInfo.Price,
                productInfo.Price * order.PurchaseQuantity,
                order.OrderDate
                );
        }

        private string BuildOrderEmailBody(int id, string name, string volume, string type, int purchaseQuantity, decimal unitPrice, decimal totalPrice,
           DateTime orderedDate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1><strong> Информация о заказе</strong></h1>");
            sb.AppendLine($"<p><strong>Id Заказа: </strong> {id}</p>");
            sb.AppendLine("<h2>Order Item: </h2>");
            sb.AppendLine("ul");
            sb.AppendLine($"<li> Название: {name} </li>");
            sb.AppendLine($"<li> Цена: {unitPrice} </li>");
            sb.AppendLine($"<li> Объем: {volume} </li>");
            sb.AppendLine($"<li> Тип: {type} </li>");
            sb.AppendLine($"<li> Количество: {purchaseQuantity} </li>");
            sb.AppendLine($"<li> Итоговая цена: {totalPrice} </li>");
            sb.AppendLine($"<li> Дата: {orderedDate} </li>");
            sb.AppendLine($"</ul>");

            sb.AppendLine("<p> Спасибо заказ!</>");
            return sb.ToString();
        }

        private async Task ClearOrderTable()
        {
            context.Orders.Remove(await context.Orders.FirstOrDefaultAsync());
            await context.SaveChangesAsync();
        }

    }
}
