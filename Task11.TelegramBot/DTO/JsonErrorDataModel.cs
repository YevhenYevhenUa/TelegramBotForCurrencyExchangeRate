using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Task11.TelegramBot.DTO;
public class JsonErrorDataModel
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
