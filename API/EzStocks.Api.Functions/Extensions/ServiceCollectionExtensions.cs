using Azure.Core;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using EzStocks.Api.Application.Observability;
using EzStocks.Api.Functions.Security;
using EzStocks.Api.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

namespace EzStocks.Api.Functions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region JwtBearerEvents
        //private static JwtBearerEvents CreateJwtBearerEvents()
        //{
        //    return new JwtBearerEvents
        //    {
        //        OnMessageReceived = async ctx =>
        //        {
        //            var token = ctx?.Request.Headers.Authorization.ToString();
        //            string path = ctx?.Request.Path ?? "";
        //            if (!string.IsNullOrEmpty(token))

        //            {
        //                Console.WriteLine("Access token");
        //                Console.WriteLine($"URL: {path}");
        //                Console.WriteLine($"Token: {token}\r\n");
        //            }
        //            else
        //            {
        //                Console.WriteLine("Access token");
        //                Console.WriteLine("URL: " + path);
        //                Console.WriteLine("Token: No access token provided\r\n");
        //            }
        //            await Task.CompletedTask;
        //        },
        //        OnTokenValidated = async ctx =>
        //        {
        //            Console.WriteLine(ctx.Options);
        //            if (ctx?.Principal != null)
        //            {
        //                foreach (var claim in ctx.Principal.Claims)
        //                {
        //                    Console.WriteLine($"{claim.Type} - {claim.Value}");
        //                }
        //            }
        //            await Task.CompletedTask;
        //        },
        //    };
        //}
        #endregion

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFunctionsAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtFunctionsBearer(options =>
                {
                    //options.Events = CreateJwtBearerEvents();
                    var section = configuration.GetRequiredSection("EntraB2C");
                    var entraB2CSettings = 
                        new EntraB2CSettings 
                        {
                            TenantId = section.GetValue<string>("TenantId"),
                            ClientId = section.GetValue<string>("ClientId")
                        };
                    var authority = $"https://login.microsoftonline.com/{entraB2CSettings.TenantId}";
                    options.Authority = authority;
                    options.Audience = $"api://{entraB2CSettings.ClientId}";

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = $"https://{entraB2CSettings.TenantId}.ciamlogin.com/{entraB2CSettings.TenantId}/v2.0",
                        ValidAudience = entraB2CSettings.ClientId
                    };
                });
                services.AddFunctionsAuthorization(options =>
                {
                    options.AddPolicy("AuthenticatedUser", policy => policy.RequireAuthenticatedUser());
                });
            return services;
        }

        public static IServiceCollection ConfigureAzureClients(this IServiceCollection services, HostBuilderContext ctx)
        {
            var configuration = ctx.Configuration;
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddServiceBusClient(configuration.GetSection("ServicebusConnection"));
                TokenCredential credential;
                if (ctx.HostingEnvironment.IsDevelopment())
                {
                    credential = new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            ExcludeEnvironmentCredential = true,
                            ExcludeManagedIdentityCredential = true,
                            ExcludeWorkloadIdentityCredential = true,
                        });
                }
                else
                {
                    credential = new ManagedIdentityCredential();
                }
                clientBuilder.UseCredential(credential);
            });
            return services;
        }

        public static IServiceCollection AddOption<TOption>(this IServiceCollection services, IConfiguration configuration, string configurationSectionKey) where TOption : class
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
