using ECommercelib.SharedLibrary.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Repositories;
using OrderApi.Presentation.Consumer;
using ProductApi.Infrastructure.Data;

namespace OrderApi.Infrastructure.Data.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //add db connedtivity
            //Add auth scheme

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
                    configurator.Host(new Uri(config["MessageBroker:Host"]!), h =>
                    {
                        h.Username(config["MessageBroker:Username"]!);
                        h.Password(config["MessageBroker:Password"]!);
                    });

                    configurator.ReceiveEndpoint("product-queue", e =>
                    {
                        e.ConfigureConsumer<ProductConsumer>(context);
                    });
                });
            });


            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:FileName"]!);

            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            //register middleweare
            //Global exception -> hansle exteranl errors
            //listen to only Gateway -> blck all outsider calls
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
