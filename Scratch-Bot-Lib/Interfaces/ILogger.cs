using Discord;

using System.Threading.Tasks;

namespace Scratch_Bot_Lib
{
    public interface ILogger
    {
        Task Log(LogMessage message);

        Task Log(string message, LogSeverity severity);

        public string FormatMsg(LogMessage msg);

        public string FormatMsg(string msg, LogSeverity severity);
    }
}
