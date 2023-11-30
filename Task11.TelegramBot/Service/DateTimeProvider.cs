using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task11.TelegramBot.Service_Interface;

namespace Task11.TelegramBot.Service;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset TimeNow => DateTime.Now;
    
}
