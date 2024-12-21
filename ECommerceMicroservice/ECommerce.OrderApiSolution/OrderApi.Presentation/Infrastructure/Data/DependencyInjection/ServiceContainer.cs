using ECommercelib.SharedLibrary.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Infrastructure.Repositories;
using OrderApi.Presentation.Consumer;
using ProductApi.Infrastructure.Data;

namespace OrderApi.Infrastructure.Data.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IOrderService, OrderService>(options =>
            {
                options.Timeout = TimeSpan.FromSeconds(10);

            });
            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<ProductConsumer>();
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    var hostSettings = config.GetSection("MessageBroker");
                    var uri = new Uri(hostSettings["Host"]!);
                    configurator.Host(uri, host =>
                    {
                        host.Username(hostSettings["Username"]!);
                        host.Password(hostSettings["Password"]!);
                    });

                    configurator.ReceiveEndpoint("product-queue", e =>
                    {
                        e.ConfigureConsumer<ProductConsumer>(context);
                    });
                });
            });

            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:FileName"]!);

            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }

    }
}
