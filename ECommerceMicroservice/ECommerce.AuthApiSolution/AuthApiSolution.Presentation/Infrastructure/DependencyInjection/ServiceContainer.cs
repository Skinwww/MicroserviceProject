using AuthApiSolution.Application.Interfaces;
using AuthApiSolution.Infrastructure.Data;
using AuthApiSolution.Infrastructure.Repositories;
using ECommercelib.SharedLibrary.DependencyInjection;
using MassTransit;
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
            SharedServiceContainer.AddSharedServices<AuthDbContext>(services, config, config["MySerilog:FileName"]!);

            services.AddScoped<IUser, UserRepository> ();
            
            return services;
        }

    }
}
