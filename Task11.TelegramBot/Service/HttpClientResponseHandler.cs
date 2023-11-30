using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Task11.TelegramBot.DTO;

namespace Task11.TelegramBot.Service;
public class HttpClientResponseHandler : IHttpClientResponseHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpClientResponseHandler> _logger;

    public HttpClientResponseHandler(IHttpClientFactory httpClientFactory, ILogger<HttpClientResponseHandler> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public async Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Send request to PB api");

        using HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode(); 

        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        T result = JsonConvert.DeserializeObject<T>(json); 

        return result;
    }

}
