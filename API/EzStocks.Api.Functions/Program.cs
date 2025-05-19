using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Utils;
using EzStocks.Api.Functions.Extensions;
using EzStocks.Api.Infrastructure.PolygonIO;
using EzStocks.Api.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scrutor;
using Serilog;

Log.Logger logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        //builder.UseFunctionsAuthorization();
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
            .AddOption<PolygonIOSettings>(configuration, PolygonIOSettings.ConfigurationSection);

        services.AddAutoMapper(EzStocks.Api.Application.AssemblyReference.Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(EzStocks.Api.Application.AssemblyReference.Assembly));

        services
            .ConfigureAzureClients(ctx)
            .ConfigureEFCore(configuration)
            .ConfigureOpenTelemetry();
            //.ConfigureAuthentication(configuration);

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

//using (var scope = host.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<EzStockDbContext>();
//    await dbContext.Database.EnsureCreatedAsync();
//}

host.Run();