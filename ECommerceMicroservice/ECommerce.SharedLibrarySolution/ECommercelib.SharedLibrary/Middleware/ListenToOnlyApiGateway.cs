using Microsoft.AspNetCore.Http;
namespace ECommercelib.SharedLibrary.Middleware
{
    public class ListenToOnlyApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {

            var signedHeader = context.Request.Headers["Api-Gateway"];

            //NULL, нет запросов от api
            if (signedHeader.FirstOrDefault() != null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Извините, сервис недоступен");
                return;
            }
            else
            {
                await next(context);

            }
        }
    }
}
