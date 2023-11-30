using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Task11.TelegramBot.DTO;
using Task11.TelegramBot.DTO.NewFolder;
using Task11.TelegramBot.Service_Interface;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Task11.TelegramBot.Service;
public class ActionHandlerService : IActionHandlerService
{
    private readonly IDataValidatorsService _dataValidatorsService;
    private readonly IHttpClientResponseHandler _httpHandler;
    private readonly AppSettings _appSettings;
    private readonly ILogger<ActionHandlerService> _logger;
    private readonly IDataFormattingService _dataFormattingService;
    private const string StartMessage = "To start, send a message with the currency code and date if you want to get data not for today.\nUse a dot as a separator for the date.";
    private const string ResponseForNonTextMessage = "This bot only responds to text messages";
    private const string DateLessOrEqualError = "wrong date, the date should be equal or less than today's date.";
    private const string DateParseError = "Your date cannot be parse, something went wrong, re-enter data.";
    private const string CurrencyCodeError = "Currency code not found, check correctness and try again";

    public ActionHandlerService(IDataValidatorsService dataValidatorsService,
        IHttpClientResponseHandler jsonHandler,
        IOptions<AppSettings> options,
        ILogger<ActionHandlerService> logger, 
        IDataFormattingService dataFormattingService)
    {
        _dataValidatorsService = dataValidatorsService;
        _httpHandler = jsonHandler;
        _appSettings = options.Value;
        _logger = logger;
        _dataFormattingService = dataFormattingService;
    }

    public Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        _logger.LogInformation($"User: {message.Chat.Username ?? "anon"} | message: {message.Text} | Date: {DateTime.Now}");
        if (message.Text is not null && message.Text.Contains("/start"))
        {
            _logger.LogInformation("Received start mcommand");
            await SendResponseToUser(botClient, message, StartMessage, cancellationToken);
            return;
        }

        if (message.Text is not null)
        {
            await ProcessResponseToTextMessage(botClient, message, cancellationToken);
            return;
        }
        else
        {
            _logger.LogInformation("Message received from user is not in text format");
            await SendResponseToUser(botClient, message, ResponseForNonTextMessage, cancellationToken);
        }
    }

    private async Task<Message> ProcessResponseToTextMessage(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken)
    {
        var dataCurrencyRecord = _dataFormattingService.GetMessageRecord(message.Text);
        string resultString = _dataValidatorsService.CheckUserInputMessage(dataCurrencyRecord.Currency);
        if (!string.IsNullOrEmpty(resultString))
        {
            return await SendResponseToUser(botClient, message, resultString, cancellationToken);
        }

        if (dataCurrencyRecord.Date.Equals(default(DateTime)))
        {
            return await SendResponseToUser(botClient, message, DateParseError, cancellationToken);
        }

        var dateRequest = _dataFormattingService.GetValidatedDate(dataCurrencyRecord);
        bool isEqualOrLessThanTodayDate = _dataValidatorsService.IsTheDateLessOrEqualToNow(dateRequest);
        if (!isEqualOrLessThanTodayDate )
        {
            return await SendResponseToUser(botClient, message, DateLessOrEqualError, cancellationToken);
        }

        var url = _appSettings.PbApiUrlRequest + dateRequest.ToShortDateString();
        var jsonResponse = await _httpHandler.GetJsonAsync<Root>(url, cancellationToken);
        var concreteCurrencyInfo = jsonResponse.ExchangeRate.Where(c => c.Currency == dataCurrencyRecord.Currency).FirstOrDefault();
        if (concreteCurrencyInfo is null)
        {
            return await SendResponseToUser(botClient, message, CurrencyCodeError, cancellationToken);
        }

        string textResult = _dataFormattingService.GetResultString(concreteCurrencyInfo, dateRequest);
        return await SendResponseToUser(botClient, message, textResult, cancellationToken);

    }

    public async Task<Message> SendResponseToUser(ITelegramBotClient botClient, Message? message, string result, CancellationToken cancellationToken)
    {
        var messageResult = await botClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: cancellationToken);
        _logger.LogInformation("The message has been sent. End of cycle.");
        return messageResult;
    }

}
