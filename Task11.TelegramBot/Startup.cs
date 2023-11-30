using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Task11.TelegramBot.DTO;
using Task11.TelegramBot.Service;

using Telegram.Bot;

namespace Task11.TelegramBot;
public class Startup : IStartup
{
    private readonly IActionHandlerService _actionHandler;
    private readonly CancellationTokenSource _tokenSource;
    private readonly IOptions<AppSettings> _options;
    private readonly ILogger<Startup> _logger;
    private readonly TelegramBotClient _botClient;

    public Startup(IActionHandlerService actionHandler,
        IOptions<AppSettings> options, ILogger<Startup> logger)
    {
        _tokenSource = new CancellationTokenSource();
        _actionHandler = actionHandler;
        _options = options;
        _logger = logger;
        _botClient = new TelegramBotClient(_options.Value.BotToken);
    }

    public async void Run()
    {
        var cultureInfo = new CultureInfo("uk-UA");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        var token = _tokenSource.Token;
        _logger.LogInformation($"The bot is up and running | Date of start of operation: {DateTime.Now}");
        _botClient.StartReceiving(_actionHandler.UpdateHandler, _actionHandler.ErrorHandler, null, cancellationToken: token);
        await Console.In.ReadLineAsync();
    }
}
