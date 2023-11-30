using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task11.TelegramBot.DTO.NewFolder;
public class Root
{
    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("bank")]
    public string Bank { get; set; }

    [JsonProperty("baseCurrency")]
    public int BaseCurrency { get; set; }

    [JsonProperty("baseCurrencyLit")]
    public string BaseCurrencyLit { get; set; }

    [JsonProperty("exchangeRate")]
    public List<ExchangeRate> ExchangeRate { get; set; }
}
