using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task11.TelegramBot.DTO.NewFolder;
public class ExchangeRate
{
    [JsonProperty("baseCurrency")]
    public string BaseCurrency { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("saleRateNB")]
    public double SaleRateNB { get; set; }

    [JsonProperty("purchaseRateNB")]
    public double PurchaseRateNB { get; set; }

    [JsonProperty("saleRate")]
    public double? SaleRate { get; set; }

    [JsonProperty("purchaseRate")]
    public double? PurchaseRate { get; set; }
}
