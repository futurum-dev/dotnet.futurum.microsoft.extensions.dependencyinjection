using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.Microsoft.Extensions.DependencyInjection.Sample;

using Serilog;

try
{
    var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                         .AddEnvironmentVariables();

    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configurationBuilder.Build())
                                          .Enrich.FromLogContext()
                                          .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                                          .CreateBootstrapLogger();
    
    Log.Information("Application starting up");

    var builder = Host.CreateDefaultBuilder(args);

    builder.UseSerilog((hostBuilderContext, loggerConfiguration) =>
                           loggerConfiguration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                                              .ReadFrom.Configuration(hostBuilderContext.Configuration));

    builder.ConfigureServices(serviceCollection =>
    {
        serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionSample();
        
        serviceCollection.AddModule<ManualModule>();
        
        serviceCollection.AddStartable<ManualStartable>();

        serviceCollection.AddSingleton<IService1, ManualService>();
    });

    var host = builder.Build();

    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application start-up failed");
}
finally
{
    Log.Information("Application shut down complete");
    Log.CloseAndFlush();
}