using Azure.Monitor.OpenTelemetry.AspNetCore;
using EzStocks.Api.Application.Observability;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Logs;

namespace EzStocks.Api.Functions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOption<TOption>(this IServiceCollection services, IConfiguration configuration, string configurationSectionKey) where TOption:class
        {
            services
                .AddOptions<TOption>()
                .Bind(configuration.GetSection(configurationSectionKey))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            return services;
        }

        public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services)
        {
            services.Configure<OpenTelemetryLoggerOptions>(options =>
            {
                options.IncludeScopes = true;
            });

            services.AddOpenTelemetry()
                .UseAzureMonitor()
                .WithTracing(opt => opt.AddSource(Traces.DefaultSource.Name));

            return services;
        }
    }
}
