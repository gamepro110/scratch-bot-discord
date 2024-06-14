using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib
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

        /// <summary>
        /// Log using a specific ILogger
        /// </summary>
        /// <typeparam name="T">T = Class : ILogger </typeparam>
        /// <param name="message">message to log</param>
        public async Task Log<T>(string message) where T : ILogger
        {
            foreach (ILogger item in Loggers)
            {
                if (item.GetType() == typeof(T))
                {
                    await item.Log(message + '\n', Settings.BotLogLevel);
                }
            }
        }

        private readonly List<ILogger> Loggers;
    }
}
