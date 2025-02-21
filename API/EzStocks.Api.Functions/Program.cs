using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scrutor;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddAutoMapper(EzStocks.Api.Application.AssemblyReference.Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(EzStocks.Api.Application.AssemblyReference.Assembly));

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
    .Build();

host.Run();
