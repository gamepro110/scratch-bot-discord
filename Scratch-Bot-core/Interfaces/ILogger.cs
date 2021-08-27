using Discord;

namespace Scratch_Bot_core
{
    public interface ILogger
    {
        Task Log(LogMessage message);
        Task Log(string message, LogSeverity severity);
    }
}
