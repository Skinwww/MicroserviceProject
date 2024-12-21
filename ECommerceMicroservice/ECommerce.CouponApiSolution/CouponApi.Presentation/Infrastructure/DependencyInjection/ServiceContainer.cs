using CouponApi.Application.Interfaces;
using CouponApi.Infrastructure.Data;
using CouponApi.Infrastructure.Repositories;
using ECommercelib.SharedLibrary.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CouponApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfastructureService(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<CouponDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            SharedServiceContainer.AddSharedServices<CouponDbContext>(services, config, config["MySerilog:FileName"]!);
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
            services.AddScoped<IBonus, BonusRepository>();
            services.AddScoped<IPromoCode, PromoCodeRepository>();

            return services;
        }

    }
}
