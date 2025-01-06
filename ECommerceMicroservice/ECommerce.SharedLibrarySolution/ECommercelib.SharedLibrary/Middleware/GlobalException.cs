using ECommercelib.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ECommercelib.SharedLibrary.Middleware
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string message = "Ошибка сервера, попробуйте снова";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await _next(context); //вызов происходит до проверки статуса
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); //метод корректно работает

                // Таймаут запроса
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Время ожидания истекло, попробуйте снова";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }

                await ModifyHeader(context, title, message, statusCode);
                return; // Прерываем дальнейшую обработку после установки заголовка
            }

            // Проверка кода состояния после завершения вызова middleware
            if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            {
                title = "Warning";
                message = "Слишком много запросов выполнено";
                statusCode = StatusCodes.Status429TooManyRequests;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                title = "Alert";
                message = "Вы не авторизованы";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                title = "Out of access";
                message = "У вас нет прав для доступа";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(context, title, message, statusCode);
                return;
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;  //статус код ответа
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title
            }), CancellationToken.None);
        }
    }
}

