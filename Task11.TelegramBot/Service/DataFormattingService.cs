using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Task11.TelegramBot.DTO;
using Task11.TelegramBot.DTO.NewFolder;
using Task11.TelegramBot.Service_Interface;

namespace Task11.TelegramBot.Service;
public class DataFormattingService : IDataFormattingService
{
    private readonly ILogger<DataFormattingService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AppSettings _appSettings;

    public DataFormattingService(ILogger<DataFormattingService> logger, IOptions<AppSettings> options, IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _appSettings = options.Value;
    }

    public DateCurrencyRecord GetMessageRecord(string message)
    {
        _logger.LogInformation("Initial processing of the user's message");
        var trimmedString = message.Replace(" ", "").ToUpper();
        int currencyCodeLength = 3;
        bool isFullMatch = Regex.IsMatch(trimmedString, _appSettings.RegexForFullPath);
        bool isCurrencyMatch = Regex.IsMatch(trimmedString, _appSettings.RegexForCurrencyCode);
        if (isFullMatch)
        {
            string prefix = trimmedString.Substring(0, currencyCodeLength);
            string restOfString = trimmedString.Substring(currencyCodeLength);
            var restOfDateString = DateTime.TryParse(restOfString, out DateTime result);
            if (!restOfDateString)
            {
                return new DateCurrencyRecord(prefix, default(DateTime));
            }

            return new DateCurrencyRecord(prefix, result);
        }

        if (isCurrencyMatch && trimmedString.Length == currencyCodeLength)
        {
            return new DateCurrencyRecord(trimmedString, null);
        }

        return new DateCurrencyRecord(string.Empty, null);

    }

    public DateTime GetValidatedDate(DateCurrencyRecord dateCurrency)
    {
        if (!string.IsNullOrEmpty(dateCurrency.Currency) && dateCurrency.Date is null)
        {
            _logger.LogInformation("Currency code is received without date, today date is set");
            var dateTimeNow = _dateTimeProvider.TimeNow.DateTime;
            var shortDateTime = dateTimeNow.Date;
            return shortDateTime;
        }
        else
        {
            return (DateTime)dateCurrency.Date;
        }
    }

    public string GetResultString(ExchangeRate exchangeRate, DateTime date)
    {
        if (exchangeRate.SaleRate is null && exchangeRate.PurchaseRate is null)
        {
            return string.Format("Date: {0}\nonly the national bank rate is available\n{1} => {2}\nsale price NB: {3} uah,\npurchase price NB: {4} uah.", date, exchangeRate.BaseCurrency, exchangeRate.Currency, exchangeRate.SaleRateNB.ToString("f"), exchangeRate.PurchaseRateNB.ToString("f"));
        }

        return string.Format("Date: {0}\n{1} => {2}\nsale price: {3} uah,\npurchase price: {4} uah.", date.ToShortDateString(), exchangeRate.BaseCurrency, exchangeRate.Currency, exchangeRate.SaleRate, exchangeRate.PurchaseRate);

    }

}
