using ECommercelib.SharedLibrary.DependencyInjection;
using EmailWebhook.Consumer;
using EmailWebhook.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EmailWebhook.Dependency_Injection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {


            services.AddHttpClient();
            services.AddScoped<IEmailService, EmailService>();
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<WebhookConsumer>();
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    var hostSettings = config.GetSection("MessageBroker");
                    var uri = new Uri(hostSettings["Host"]!);
                    configurator.Host(uri, host =>
                    {
                        host.Username(hostSettings["Username"]!);
                        host.Password(hostSettings["Password"]!);
                    });

                    configurator.ReceiveEndpoint("email-webhook-queue", e =>
                    {
                        e.ConfigureConsumer<WebhookConsumer>(context);
                    });
                });
            });


            return services;
        }

    }
}
