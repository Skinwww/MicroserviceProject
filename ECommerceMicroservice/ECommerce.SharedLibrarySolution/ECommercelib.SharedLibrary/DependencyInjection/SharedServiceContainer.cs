using ECommercelib.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Serilog;

namespace ECommercelib.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration config, string fileName) where TContext : DbContext
        {
         
            services.AddDbContext<TContext>(option => option.UseNpgsql(
             config
             .GetConnectionString("DefaultConnection"), sqlserverOption =>
             sqlserverOption.EnableRetryOnFailure()
                ));

            //serilog logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName} -.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-dd HH:mm:ss.fff zzz} [{Level:u3}] {message :lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();


            JWTAuthScheme.AddJWTAuthenticationScheme(services, config);
            return services;
        }
        public static IApplicationBuilder UseSharedPolicies( this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();
            return app;
        }
    }
}
