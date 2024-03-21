using APIexamen1HoracioPuma.Contratos.Repositorios;
using APIexamen1HoracioPuma.Implementacion.Repositorios;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IProveedorRepositorio, ProveedorRepositorio>();
        services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
    })
    .Build();

host.Run();
