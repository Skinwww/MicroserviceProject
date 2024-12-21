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
                    configurator.Host(new Uri(config["MessageBroker:Host"]!), h =>
                    {
                        h.Username(config["MessageBroker:Username"]!);
                        h.Password(config["MessageBroker:Password"]!);
                    });
                });
            });

            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:ProductApi"]!);
            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }

   
}
