using ECommercelib.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Repositories;

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
