﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scratch_Bot_core.Modules
{
    [Group("Sudo")]
    public class SudoModule : CustomBaseModule
    {
        public SudoModule(DiscordSocketClient _socketClient, LoggingService _logging)
        {
            socketClient = _socketClient;
            logging = _logging;
        }

        [Command("Ban")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IGuildUser user, [Remainder] string? reason = null)
        {
            await user.Guild.AddBanAsync(user, 0, reason);
            await ReplyAsync("ok!");
        }

        [Command("purge")]
        [Summary("Cleanup x messages. (calling message will also be cleaned up, default = 10)")]
        [RequireUserPermission(ChannelPermission.ManageMessages), RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeMessages(int amount = 10)
        {
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
                    await (Context.Channel as ITextChannel).DeleteMessagesAsync(_filteredMessages);//deletes previous messages
                    await (Context.Channel as ITextChannel).DeleteMessageAsync(Context.Message);//deletes calling messages

                    _em.Color = Color.Green;
                    _em.Title = "did thing. you proud??";
                    _em.Description = $"deleted {_filteredCount + 1} {(_filteredCount > 1 ? "messages" : "message")}.";
                }
            }

            await ReplyAsync(embed: _em.Build());
        }

        #region ping
        [Command("Ping")]
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
            public async Task webPing(string url = "google.com", int numPing = 4)
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

        DiscordSocketClient socketClient;
        LoggingService logging;
    }
}
