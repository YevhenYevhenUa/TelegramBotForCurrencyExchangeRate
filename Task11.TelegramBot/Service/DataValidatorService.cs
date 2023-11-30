using Microsoft.Extensions.Logging;
using Task11.TelegramBot.Service_Interface;

namespace Task11.TelegramBot.Service;
public class DataValidatorService : IDataValidatorsService
{
    private readonly ILogger<DataValidatorService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private const string ErrorResult = "Wrong message type, try again! Use pattern 'USD dd.mm.yyyy' for correct response";
    public DataValidatorService(ILogger<DataValidatorService> logger, IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public string CheckUserInputMessage(string curency)
    {
        if (string.IsNullOrEmpty(curency))
        {
            _logger.LogInformation("Message type is incorrect! Further processing has been halted");
            return ErrorResult;
        }

        return string.Empty;
    }

    public bool IsTheDateLessOrEqualToNow(DateTime date)
    {
        var dateTimeNow = _dateTimeProvider.TimeNow.DateTime;
        int res = DateTime.Compare(date, dateTimeNow);
        if (res == 1 )
        {
            return false;
        }

        return true;
    }
   
}
