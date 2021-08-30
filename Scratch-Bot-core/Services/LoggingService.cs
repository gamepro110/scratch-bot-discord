using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scratch_Bot_core
{
    public class LoggingService
    {
        public LoggingService(IEnumerable<ILogger> loggers)
        {
            Loggers = new List<ILogger>(loggers);
        }

        public Task Log(LogMessage message)
        {
            for (int i = 0; i < Loggers.Count; i++)
            {
                Loggers[i].Log(message);
            }
            return Task.Delay(1);
        }

        public Task Log(string message, LogSeverity severity = LogSeverity.Info)
        {
            for (int i = 0; i < Loggers.Count; i++)
            {
                Loggers[i].Log(message, severity);
            }
            return Task.Delay(1);
        }

        private readonly List<ILogger> Loggers;
    }
}
