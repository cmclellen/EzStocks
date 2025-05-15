using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Utils;
using EzStocks.Api.Functions.Extensions;
using EzStocks.Api.Infrastructure.Alphavantage;
using EzStocks.Api.Infrastructure.PolygonIO;
using EzStocks.Api.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scrutor;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        // Explicitly adding the extension middleware because
        // registering middleware when extension is loaded does not
        // place the middleware in the pipeline where required request
        // information is available.
        builder.UseFunctionsAuthorization();
    })
    .ConfigureAppConfiguration((ctx, config) =>
    {
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables().AddUserSecrets(typeof(Program).Assembly);
    })
    .ConfigureServices((ctx, services) =>
    {
        var configuration = ctx.Configuration;

        services
            .AddOption<AlphavantageSettings>(configuration, AlphavantageSettings.ConfigurationSection)
            .AddOption<PolygonIOSettings>(configuration, PolygonIOSettings.ConfigurationSection);

        services.AddAutoMapper(EzStocks.Api.Application.AssemblyReference.Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(EzStocks.Api.Application.AssemblyReference.Assembly));

        services
            .ConfigureAzureClients(ctx)
            .ConfigureEFCore(configuration)
            .ConfigureOpenTelemetry();

        services.AddFunctionsAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtFunctionsBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async ctx =>
                    {
                        var token = ctx?.Request.Headers.Authorization.ToString();
                        string path = ctx?.Request.Path ?? "";
                        if (!string.IsNullOrEmpty(token))

                        {
                            Console.WriteLine("Access token");
                            Console.WriteLine($"URL: {path}");
                            Console.WriteLine($"Token: {token}\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Access token");
                            Console.WriteLine("URL: " + path);
                            Console.WriteLine("Token: No access token provided\r\n");
                        }
                        await Task.CompletedTask;
                    },
                    OnTokenValidated = async ctx =>
                    {
                        Console.WriteLine(ctx.Options);
                        if (ctx?.Principal != null)
                        {
                            foreach (var claim in ctx.Principal.Claims)
                            {
                                Console.WriteLine($"{claim.Type} - {claim.Value}");
                            }
                        }
                        await Task.CompletedTask;
                    },
                };

                var tenantId = "a29a997e-a4fc-4e83-ae12-d78f0c8a0443";
                var clientId = "00897edf-d475-4485-b036-c10f7515c6ad";
                var authority = $"https://login.microsoftonline.com/{tenantId}";
                options.Authority = authority;
                options.Audience = $"api://{clientId}";

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = $"https://{tenantId}.ciamlogin.com/{tenantId}/v2.0",
                    ValidAudience = clientId
                };
            });
        services.AddFunctionsAuthorization(options =>
        {
            options.AddPolicy("AuthenticatedUser", policy => policy.RequireAuthenticatedUser());
        });

        services.AddScoped<IStocksApiClient, PolygonIOStocksApiClient>();        

        services.Scan(selector => selector
            .FromAssemblies(
                EzStocks.Api.Functions.AssemblyReference.Assembly,
                EzStocks.Api.Application.AssemblyReference.Assembly,
                EzStocks.Api.Domain.AssemblyReference.Assembly,
                EzStocks.Api.Persistence.AssemblyReference.Assembly,
                EzStocks.Api.Infrastructure.AssemblyReference.Assembly)
            .AddClasses(f=>f.Where(t=>!t.IsAssignableTo(typeof(IDateTimeProvider))), false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithTransientLifetime());
    })
    .ConfigureLogging(logging =>
    {
        logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            LoggerFilterRule defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName
                == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider")!;
            if (defaultRule is not null)
            {
                options.Rules.Remove(defaultRule);
            }
        });
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EzStockDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

host.Run();