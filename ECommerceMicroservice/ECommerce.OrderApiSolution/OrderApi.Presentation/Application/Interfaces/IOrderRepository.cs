﻿using ECommercelib.SharedLibrary.Interface;
using ECommercelib.SharedLibrary.Responses;
using OrderApi.Presentation.Application.DTOs;
using OrderApi.Presentation.Domain.Entities;
using System.Linq.Expressions;

namespace OrderApi.Application.Interfaces
{
    public interface IOrderRepository: IGenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
        Task<Response> UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<OrderSummaryDTO> GetOrderSummaryAsync(int orderId);
    }
}