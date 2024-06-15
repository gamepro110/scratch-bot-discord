using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib.Modules
{
    [Group("Sudo")]
    public class SudoModule : CustomBaseModule
    {
        public SudoModule(DiscordSocketClient _socketClient, LoggingService _loggingService, CancellationTokenSource _cancellationTokenSource)
        {
            socketClient = _socketClient;
            loggingService = _loggingService;
            mainCancellationToken = _cancellationTokenSource;
        }

        [Command("Ban")]
        [Summary("ban a user, reason why can be given after the name inside \"\"")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IGuildUser user, [Remainder] string? reason = null)
        {
            await user.Guild.AddBanAsync(user, 0, reason);
            await ReplyAsync("ok!");
        }

        [Command("Purge")]
        [Summary("Cleanup x messages, calling message will also be cleaned up. (default = 10)")]
        [RequireUserPermission(ChannelPermission.ManageMessages), RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeMessages(int amount = 10)
        {
            // TODO add functionality to delete messages older than 14 days using paralel(?) for loop
            // TODO make deleting calling message optional
            EmbedBuilder _em = new();

            if (amount <= 0)
            {
                _em.Color = Color.DarkBlue;
                _em.Title = "cant delete anything if you dont give me an amount to delete...";
            }
            else
            {
                IEnumerable<IMessage> _messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount).FlattenAsync();
                IEnumerable<IMessage> _filteredMessages = _messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14); // trying to bulk delete messages older than 14 days will result in a bad request!!
                int _filteredCount = _filteredMessages.Count();

                if (_filteredCount == 0)
                {
                    _em.Color = Color.DarkBlue;
                    _em.Title = "got nothing to delete tho...";
                }
                else
                {
                    await ((ITextChannel)Context.Channel).DeleteMessagesAsync(_filteredMessages);//deletes previous messages
                    await ((ITextChannel)Context.Channel).DeleteMessageAsync(Context.Message);//deletes calling messages

                    _em.Color = Color.Green;
                    _em.Title = "did thing. you proud??";
                    _em.Description = $"deleted {_filteredCount + 1} {(_filteredCount > 1 ? "messages" : "message")}.";
                }
            }

            await ReplyAsync(embed: _em.Build());
        }

        [Command("Remind")]
        public async Task SendReminder(string reminder) =>
            await loggingService.Log<WebhookLoggingService>(reminder);

        [Command("LogError")]
        public async Task LogToLogFile(string message) =>
            await loggingService.Log<FileLoggingService>(message);

        [Command("Shutdown")]
        public async Task Shutdown(string reason = "")
        {
            if (!string.IsNullOrWhiteSpace(reason))
            {
                await LogToLogFile(reason);
            }

            await socketClient.LogoutAsync();
            await socketClient.StopAsync();
            mainCancellationToken.Cancel();
        }

        #region ping
        [Command("Ping")]
        [Summary("send out a ping <amountOfPings> requests to <URL> and get the bots latency")]
        public async Task PingAll(string url = "google.com", int amountOfPings = 4)
        {
            EmbedBuilder builder = new();

            builder.Title = "ping";
            builder.Description = "just checkin my pings";

            builder.AddField(f =>
            {
                f.Name = "Bot ping";
                f.Value = $"{socketClient.Latency} ms";
            });

            PingRequest request = new();
            await request.Send(url, amountOfPings);

            builder.AddField(f =>
            {
                f.Name = "Web ping";
                f.Value = string.Format(
                    "(send/received/accuracy): ({0}/{1}/{2:0.00})\n(total/avg): ({3}/{4:0.0}) ms",
                    request.plannedTrips,
                    request.succesfulTrips,
                    request.accuracy,

                    request.totalTime,
                    request.averageTime
                    );
            });

            await SendEmbed(builder);
        }

        [Group("ping")]
        public class PingModule : CustomBaseModule
        {
            public PingModule(DiscordSocketClient discordSocketClient)
            {
                socketClient = discordSocketClient;
            }

            [Command("Bot")]
            [Summary("get the bots latency")]
            public async Task BotPing()
            {
                EmbedBuilder builder = new()
                {
                    Title = "Bot ping",
                    Description = $"last bot ping: {socketClient.Latency} ms",
                };

                await SendEmbed(builder);
            }

            [Command("Web")]
            [Summary("send out a ping <amountOfPings> requests to <URL>")]
            public async Task WebPing(string url = "google.com", int numPing = 4)
            {
                PingRequest request = new();
                await request.Send(url, numPing);

                string txt = "";
                txt += $"requests (send/recieved/accuracy): ({request.plannedTrips}/{request.succesfulTrips}/{(float)request.succesfulTrips / request.plannedTrips})";
                txt += $"time (total/avg): ({request.totalTime}/{request.totalTime / request.succesfulTrips})";

                EmbedBuilder builder = new()
                {
                    Title = $"ping {url}",
                    Description = txt,
                };

                await SendEmbed(builder);
            }

            private readonly DiscordSocketClient socketClient;
        }
        #endregion

        private readonly DiscordSocketClient socketClient;
        private readonly LoggingService loggingService;
        private readonly CancellationTokenSource mainCancellationToken;
    }
}
