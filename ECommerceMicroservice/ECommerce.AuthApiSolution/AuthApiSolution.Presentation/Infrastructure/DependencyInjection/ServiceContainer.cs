using AuthApiSolution.Application.Interfaces;
using AuthApiSolution.Infrastructure.Data;
using AuthApiSolution.Infrastructure.Repositories;
using ECommercelib.SharedLibrary.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace AuthApiSolution.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfastructureService(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });


            SharedServiceContainer.AddSharedServices<AuthDbContext>(services, config, config["MySerilog:FileName"]!);

            services.AddScoped<IUser, UserRepository> ();
            
            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
