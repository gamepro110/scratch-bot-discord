using Discord;

using Scratch_Bot_Lib;

namespace scratch_bot_tests
{
    public class TestConsoleLoggingService
    {
        [SetUp]
        public void Setup()
        {
            logger = new ConsoleLoggingService();
        }

        [TestCase(LogSeverity.Info, "logTest", "log-info", ExpectedResult = "(Info):[(MSG) log-info (SRC) logTest]\n")]
        [TestCase(LogSeverity.Debug, "logTest", "log-info", ExpectedResult = "(Debug):[(MSG) log-info (SRC) logTest]\n")]
        public string TestFormatMsg_msg(LogSeverity severity, string source, string msg)
        {
            LogMessage message = new(severity, source, msg);
            return logger.FormatMsg(message);
        }

        [TestCase("log-info", LogSeverity.Info, ExpectedResult = "(Info):[log-info]\n")]
        [TestCase("log-info", LogSeverity.Debug, ExpectedResult = "(Debug):[log-info]\n")]
        public string TestFormatMsg_str_sev(string msg, LogSeverity severity)
        {
            return logger.FormatMsg(msg, severity);
        }

        private ConsoleLoggingService logger;
    }
}