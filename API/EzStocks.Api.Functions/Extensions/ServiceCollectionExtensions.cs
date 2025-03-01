using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
