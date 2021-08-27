using Discord;
using System.Net;

namespace Scratch_Bot_core
{
    public class WebhookLoggingService : BaseLogger
    {
        public override async Task Log(LogMessage message)
        {
            await Log(FormatMsg(message), message.Severity);
        }

        public override async Task Log(string message, LogSeverity severity)
        {
            if (severity < Settings.BotLogLevel)
            {
                using HttpClient client = new();

                Dictionary<string, string?> querry = new()
                {
                    { "username", "scratch-webhook-log" },
                    { "content", FormatMsg(message, severity) }
                };

                FormUrlEncodedContent postData = new(querry);

                var response = await client.PostAsync(Settings.WebhookUrl, postData);

                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    // TODO log error code to file
                }

                await Task.CompletedTask;
            }
        }
    }
}
