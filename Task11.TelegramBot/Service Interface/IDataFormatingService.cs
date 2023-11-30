using Task11.TelegramBot.DTO;
using Task11.TelegramBot.DTO.NewFolder;

namespace Task11.TelegramBot.Service_Interface;
public interface IDataFormattingService
{
    DateTime GetValidatedDate(DateCurrencyRecord dateCurrency);
    DateCurrencyRecord GetMessageRecord(string message);
    string GetResultString(ExchangeRate exchangeRate, DateTime date);
}
