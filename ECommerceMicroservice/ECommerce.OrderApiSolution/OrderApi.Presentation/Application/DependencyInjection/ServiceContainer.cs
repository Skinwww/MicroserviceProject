using ECommercelib.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using ProductApi.Infrastructure.Data;

namespace OrderApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            

            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 1, 
                Delay = TimeSpan.FromSeconds(10),
                OnRetry = args =>
                {
                    string message = $"OnRetry, попытка: {args.AttemptNumber} Результат {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }
            };

        
            //retry
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy);
            });

            return services;
        }
    }
}
