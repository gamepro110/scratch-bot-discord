using Scratch_Bot_core.Modules;
using Discord;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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

                List<KeyValuePair<string?, string?>> querry = new()
                {
                    new KeyValuePair<string?, string?>("username", "scratch-webhook-log"),
                    new KeyValuePair<string?, string?>("content", FormatMsg(message, severity)),
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
