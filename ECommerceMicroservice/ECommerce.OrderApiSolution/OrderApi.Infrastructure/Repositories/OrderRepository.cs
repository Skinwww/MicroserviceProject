
using ECommercelib.SharedLibrary.Logs;
using ECommercelib.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;
using System.Linq.Expressions;


namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                return order.Id > 0 ? new Response(true, "Заказ успешно оформлен") :
                new Response(false, "Появилась ошибка при оформлении заказа");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Появилась ошибка при оформлении заказа");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order is null)
                    return new Response(false, "Заказ не найден");

                context.Orders.Remove(entity);
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

    }
}
