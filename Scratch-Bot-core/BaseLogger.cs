using Discord;

namespace Scratch_Bot_core
{
    public abstract class BaseLogger : ILogger
    {
        bool TwoLine => logFormat == LogFormat.twoLine;
        bool FourLine => logFormat == LogFormat.fourLine;

        protected string FormatMsg(LogMessage msg)
        {
            // prints message and source, adds exception if not null
            return FormatMsg(
                string.Format(
                    "(MSG) {0} " +
                    "(SRC) {1} " +
                    "{2} ",
                    msg.Message,
                    msg.Source,
                    msg.Exception == null ? "" : "(EXCEPTION) " + msg.Exception.Message
                    ),
                msg.Severity
                );
        }

        protected string FormatMsg(string msg, LogSeverity severity)
        {
            return string.Format(
                "({0}):{2}[{3}{1}{3}]",
                severity,
                msg,
                FourLine || TwoLine ? "\n" : "",
                FourLine ? "\n\t" : ""
            );
        }

        public abstract Task Log(LogMessage message);
        public abstract Task Log(string message, LogSeverity severity);

        protected LogFormat logFormat;
    }
}
