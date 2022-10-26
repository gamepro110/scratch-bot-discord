using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;

namespace Scratch_Bot_core
{
    public class Settings
    {
        public const string TokenFile = "token.tkn";

        // Logging
        public const LogFormat BotLogFormat = LogFormat.oneLine;
        public const LogSeverity BotLogLevel = LogSeverity.Warning;
        public const string LogFile = "ScratchBot.log";

        // DiscordSocketClient
        public const bool AlwaysDownloadUsers = false;
        public const RetryMode DefaultRetryMode = RetryMode.RetryTimeouts;
        public const bool ExclusiveBuilkDelete = true;
        public const bool UseSystemClock = true;

        // Command Settings
        public const bool IsCaseSensitive = false;
        public const RunMode DefaultRunMode = RunMode.Async;
        public const bool IgnoreExtraArgs = true;
        public const char Seperator = ' ';

        public const string CommandPrefix = "$";

        private static string webhookUrl = "";

        public static string WebhookUrl
        {
            get => webhookUrl;
            set => webhookUrl = webhookUrl == string.Empty ? value : webhookUrl;
        }

        internal static readonly DiscordSocketConfig socketConfig = new()
        {
            AlwaysDownloadUsers = AlwaysDownloadUsers,
            DefaultRetryMode = DefaultRetryMode,
            UseSystemClock = UseSystemClock,
            //ExclusiveBulkDelete = ExclusiveBuilkDelete, // not available in discord.net.labs
            IdentifyMaxConcurrency = Environment.ProcessorCount / 2,
            LogLevel = BotLogLevel,
        };

        internal static readonly CommandServiceConfig commandConfig = new()
        {
            CaseSensitiveCommands = IsCaseSensitive,
            DefaultRunMode = DefaultRunMode,
            IgnoreExtraArgs = IgnoreExtraArgs,
            LogLevel = BotLogLevel,
            SeparatorChar = Seperator,
        };
    }

    public enum LogFormat
    {
        oneLine,
        twoLine,
        fourLine,
    }
}
