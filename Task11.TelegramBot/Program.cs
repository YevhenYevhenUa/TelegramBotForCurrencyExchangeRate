using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Task11.TelegramBot;
using Task11.TelegramBot.DTO;
using Task11.TelegramBot.Service;
using Task11.TelegramBot.Service_Interface;

internal class Program
{
    private static void Main(string[] args)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var configFilePath = Path.Combine(basePath, "Appsettings.json");
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(configFilePath, optional: false)
        .Build();

        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IActionHandlerService, ActionHandlerService>();
        services.AddScoped<IDataValidatorsService, DataValidatorService>();
        services.AddScoped<IHttpClientResponseHandler, HttpClientResponseHandler>();
        services.AddScoped<IStartup, Startup>();
        services.AddScoped<IDataFormattingService, DataFormattingService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddOptions<AppSettings>().Bind(configuration.GetSection("telegrambot"));
        services.AddHttpClient();
        services.AddLogging(options => options.AddConsole());
        var serviceProvider = services.BuildServiceProvider();
        var app = serviceProvider.GetRequiredService<IStartup>();
        app.Run();
    }
}