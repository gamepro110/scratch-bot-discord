using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scratch_Bot_core.Modules
{
    [Group("Voice")]
    public class VoiceChatModule : CustomBaseModule
    {
        [Command("All")]
        [Summary("gets a list of all the peeps in all voice chats")]
        public async Task GetAllUsersInGuildVC()
        {
            EmbedBuilder builder = new() 
            {
                Color = Color.DarkPurple,
            };

            foreach (SocketVoiceChannel voiceChannel in Context.Guild.VoiceChannels)
            {
                builder.AddField(f => 
                {
                    bool VcMoreThanOne = voiceChannel.Users.Count > 0;
                    f.Name = voiceChannel.Name;
                    f.Value = VcMoreThanOne ?
                    string.Join(
                        ", ",
                        voiceChannel.Users.Select(u =>
                            $"({u.Status})->{u.Nickname ?? u.Username}#{u.Discriminator}"
                        )) : 
                    "-";
                    f.Value += $"\n\ntotal: {voiceChannel.Users.Count}";
                    f.IsInline = !VcMoreThanOne;
                });
            }
            
            await ReplyAsync(embed: builder.Build());
        }

        [Command("Status")]
        [Summary("sends a list of every one in your voice chat")]
        public async Task GetUsersInRequestorVC()
        {
            EmbedBuilder builder = new()
            {
                Color = Color.DarkPurple,
                Description = string.Format(
                    "{0}",
                    DateTime.Now.ToString("yyyy/MM/dd hh:mm tt")
                ),
            };

            IGuildUser guildUser = (IGuildUser)Context.User;
            IVoiceChannel vc = guildUser.VoiceChannel;
            string txt = "";

            if (vc != null)
            {
                await foreach (IReadOnlyCollection<IGuildUser> usrList in vc.GetUsersAsync())
                {
                    foreach (IGuildUser usr in usrList)
                    {
                        txt += string.Format(
                            "({0})->{1}#{2}",
                            usr.Status,
                            usr.Nickname ?? usr.Username,
                            usr.Discriminator
                        );
                    }
                }
            }
            else
            {
                txt = "join a voice chat.";
            }
            
            builder.AddField(f => 
            {
                f.Name = $"[{(vc != null ? vc.Name : "...")}]";
                f.Value = txt;
            });

            await ReplyAsync(embed: builder.Build());
        }
    }
}
