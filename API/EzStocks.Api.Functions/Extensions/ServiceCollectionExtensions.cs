using Azure.Monitor.OpenTelemetry.AspNetCore;
using EzStocks.Api.Application.Observability;
using EzStocks.Api.Persistence;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public static IServiceCollection ConfigureEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection")!;
            services.AddDbContext<EzStockDbContext>((sp, options) => options.UseCosmos(conn, databaseName: "EzStocks"));

            return services;
        }

        public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services)
        {
            services.Configure<OpenTelemetryLoggerOptions>(options =>
            {
                options.IncludeScopes = true;
            });

            var ot = services.AddOpenTelemetry();

            ot.UseFunctionsWorkerDefaults();
            ot.UseAzureMonitor()
                .WithTracing(opt => opt.AddSource(Traces.DefaultSource.Name));

            return services;
        }
    }
}
