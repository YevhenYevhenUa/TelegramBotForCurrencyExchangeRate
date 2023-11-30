using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task11.TelegramBot.DTO;
public class AppSettings
{
    public string? BotToken { get; set; }
    public string? PbApiUrlRequest { get; set; }
    public string? RegexForFullPath { get; set; }
    public string? RegexForCurrencyCode { get; set; }
}
