using CouponApi.Application.Interfaces;
using CouponApi.Infrastructure.Data;
using CouponApi.Infrastructure.Repositories;
using ECommercelib.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            services.AddScoped<IBonus, BonusRepository>();
            services.AddScoped<IPromoCode, PromoCodeRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
