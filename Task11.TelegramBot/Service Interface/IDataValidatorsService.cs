using Task11.TelegramBot.DTO;

namespace Task11.TelegramBot.Service;
public interface IDataValidatorsService
{
    string CheckUserInputMessage(string inputMessage);
    bool IsTheDateLessOrEqualToNow(DateTime date);
}
