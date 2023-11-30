using Telegram.Bot;
using Telegram.Bot.Types;

namespace Task11.TelegramBot.Service;
public interface IActionHandlerService
{
    Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken);
}
