using Discord;
using System.Threading.Tasks;

namespace Scratch_Bot_core
{
    public abstract class BaseLogger : ILogger
    {
        private static bool TwoLine => Settings.BotLogFormat == LogFormat.twoLine;
        private static bool FourLine => Settings.BotLogFormat == LogFormat.fourLine;

        protected static string FormatMsg(LogMessage msg)
        {
            // prints message and source, adds exception if not null
            return FormatMsg(
                string.Format(
                    "(MSG) {0}" +
                    " (SRC) {1}" +
                    "{2}",
                    msg.Message,
                    msg.Source,
                    msg.Exception == null ? "" : " (EXCEPTION) " + msg.Exception.Message
                    ),
                msg.Severity
                );
        }

        protected static string FormatMsg(string msg, LogSeverity severity)
        {
            string fourOrTwoLine = FourLine || TwoLine ? "\n" : "";
            string fourline = FourLine ? "\n" : "";
            string fourlineTab = FourLine ? "\t" : "";

            return string.Format(
                "({0}):{2}[{3}{4}{1}{3}]\n",
                severity,
                msg,
                fourOrTwoLine,
                fourline,
                fourlineTab
            );
        }

        public abstract Task Log(LogMessage message);
        public abstract Task Log(string message, LogSeverity severity);
    }
}
