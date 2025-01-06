using ECommercelib.SharedLibrary.Interface;
using ECommercelib.SharedLibrary.Responses;
using OrderApi.Domain.Entities;
using System.Linq.Expressions;

namespace OrderApi.Application.Interfaces
{
    public interface IOrder: IGenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
        Task<Response> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}
