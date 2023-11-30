using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task11.TelegramBot.Service_Interface;
public interface IDateTimeProvider
{
    DateTimeOffset TimeNow { get; }
}
