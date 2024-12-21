using ECommercelib.SharedLibrary.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;


namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)

        {
            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    var hostSettings = config.GetSection("MessageBroker");
                    var uri = new Uri(hostSettings["Host"]!);
                    configurator.Host(uri, host =>
                    {
                        host.Username(hostSettings["Username"]!);
                        host.Password(hostSettings["Password"]!);
                    });
                });
            });

            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:ProductApi"]!);
            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

    }

   
}
