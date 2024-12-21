using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Presentation.Domain.Entities;
using Polly.Registry;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrderRepository orderInterface, HttpClient httpclient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        //GET product
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var getProduct = await httpclient.GetAsync($"/api/Auth/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        //get user
        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await httpclient.GetAsync($"/api/Products/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;
            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return product!;
        }
            public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
           var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null || order!.Id <= 0)
                return null!;

            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));
            if (productDTO is null)
            {
                // Обработка случая, когда продукт не найден или API вернул ошибку
                throw new Exception($"Продукт с Id {order.ProductId} не нвйден.");
            }

            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));
            if (appUserDTO is null)
            {
                throw new Exception($"Пользователь с Id {order.ClientId} не найден.");
            }
            return new OrderDetailsDTO(
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                productDTO.Volume,
                productDTO.Type,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Price * order.PurchaseQuantity,
                order.OrderDate,
                order.Status
                )
            {

            };
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if(!orders.Any()) return null!;

            var orderDTOs = OrderConversion.FromEntity(orders);
            return orderDTOs!;
        }
    }
}
