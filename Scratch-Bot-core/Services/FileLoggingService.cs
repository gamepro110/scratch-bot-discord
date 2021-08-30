using Discord;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_core
{
    public class FileLoggingService : BaseLogger
    {
        public FileLoggingService(CancellationToken _cancellationToken)
        {
            cancellationToken = _cancellationToken;
        }

        public override async Task Log(LogMessage message)
        {
            await Log(FormatMsg(message), message.Severity);
        }

        public override async Task Log(string message, LogSeverity severity)
        {
            if (severity < Settings.BotLogLevel)
            {
                await File.AppendAllTextAsync(Settings.LogFile, message, cancellationToken);
            }
        }

        readonly CancellationToken cancellationToken;
    }
}
