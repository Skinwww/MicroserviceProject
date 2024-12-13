using ECommercelib.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrder _orderInterface;
        private readonly IOrderService _orderService;

        public OrdersController(IOrder orderInterface, IOrderService orderService)
        {
            _orderInterface = orderInterface;
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _orderInterface.GetAllAsync();
            if (!orders.Any())
                return NotFound("В базе данных не найдено заказов");

            var(_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ? NotFound() : Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _orderInterface.FindByIdAsync(id);
            if (order is null)
                return NotFound(null);

            var (_order, _) = OrderConversion.FromEntity(order, null);
            return Ok(_order);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return BadRequest("Неверные данные");

            try
            {
                var orderDetail = await _orderService.GetOrderDetails(orderId);
                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Или другой подходящий метод обработки ошибок
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incomplete data submited");
            //convert to entity
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await _orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);

        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await _orderInterface.UpdateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await _orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId)
        {
            if (clientId <= 0) return BadRequest("Invalid data provided");

            var orders = await _orderService.GetOrdersByClientId(clientId);
            return !orders.Any()? NotFound(null) : Ok(orders);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<Response>> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            var response = await _orderInterface.UpdateOrderStatusAsync(id, newStatus);
            if (!response.Flag)
                return BadRequest(response);
            return Ok(response);
        }




    }
}
