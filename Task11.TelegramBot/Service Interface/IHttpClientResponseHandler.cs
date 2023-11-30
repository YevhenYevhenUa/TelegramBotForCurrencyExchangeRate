using Task11.TelegramBot.DTO;

namespace Task11.TelegramBot.Service;
public interface IHttpClientResponseHandler
{
    Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken);
}
