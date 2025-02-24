using Azure.Identity;
using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scrutor;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((ctx, config) =>
    {
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables().AddUserSecrets(typeof(Program).Assembly);
    })
    .ConfigureServices((hbctx, services) =>
    {
        var configuration = hbctx.Configuration;
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddAutoMapper(EzStocks.Api.Application.AssemblyReference.Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(EzStocks.Api.Application.AssemblyReference.Assembly));

        services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.UseCredential(new DefaultAzureCredential());
            });

        var conn = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<EzStockDbContext>((sp, options) => options.UseCosmos(conn, databaseName: "EzStocks"));

        services.Scan(selector => selector
            .FromAssemblies(
                EzStocks.Api.Application.AssemblyReference.Assembly,
                EzStocks.Api.Domain.AssemblyReference.Assembly,
                EzStocks.Api.Persistence.AssemblyReference.Assembly)
            .AddClasses(false)
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