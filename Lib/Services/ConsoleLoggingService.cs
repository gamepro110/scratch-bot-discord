using Discord;

using System;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib
{
    public class ConsoleLoggingService : BaseLogger
    {
        public override Task Log(LogMessage message)
        {
            LogConsole(FormatMsg(message), message.Severity);
            return Task.CompletedTask;
        }

        public override Task Log(string message, LogSeverity severity)
        {
            LogConsole(FormatMsg(message, severity), severity);
            return Task.CompletedTask;
        }

        private static void LogConsole(string msg, LogSeverity severity)
        {
            if (severity <= Settings.BotLogLevel)
            {
                switch (severity)
                {
                    case LogSeverity.Critical:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;

                    case LogSeverity.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;

                    case LogSeverity.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogSeverity.Info:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    case LogSeverity.Verbose:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;

                    case LogSeverity.Debug:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;

                    default:
                        Console.ResetColor();
                        break;
                }

                Console.WriteLine(msg);
                Console.ResetColor();
            }
        }
    }
}
