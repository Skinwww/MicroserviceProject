using ECommercelib.SharedLibrary.DTOs;
using MassTransit;
using System.Net.Http.Json;

namespace EmailWebhook.Consumer
{
    public class WebhookConsumer(HttpClient httpClient) : IConsumer<EmailDTO>
    {
        public async Task Consume(ConsumeContext<EmailDTO> context)
        {
            var result = await httpClient.PostAsJsonAsync("https://localhost:7046/email-webhook",
            new EmailDTO(context.Message.Title, context.Message.Content));
            result.EnsureSuccessStatusCode();
        }
    }
}
