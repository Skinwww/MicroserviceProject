using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly.Registry;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpclient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        //GET product
        public async Task<ProductDTO> GetProduct(int productId)
        {
            //call product api using http client
            //redirect this call to the api gateway since api is not responce to outsiders
            var getProduct = await httpclient.GetAsync($"/api/Auth/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        //get user
        public async Task<AppUserDTO> GetUser(int userId)
        {
            //call product api using http client
            //redirect this call to the api gateway since api is not responce to outsiders
            var getUser = await httpclient.GetAsync($"/api/Products/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;
            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return product!;
        }
            //get order details by id
            public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
           //prepare order
           var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null || order!.Id <= 0)
                return null!;

            //get retry pipeline
            // get retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            // prepare product
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));
            if (productDTO is null)
            {
                // Обработка случая, когда продукт не найден или API вернул ошибку
                throw new Exception($"Product with ID {order.ProductId} not found.");
            }

            // prepare client
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));
            if (appUserDTO is null)
            {
                // Обработка случая, когда клиент не найден или API вернул ошибку
                throw new Exception($"User with ID {order.ClientId} not found.");
            }

            //populate order Details
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
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderDate,
                order.Status
                )
            {

            };
        }

        //get orders by client id
        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            //get all client orders
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if(!orders.Any()) return null!;

            //convert from entity to dto
            var(_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;
        }
    }
}
