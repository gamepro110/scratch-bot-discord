using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Scratch_Bot_core.Modules
{
    [Group("Voice")]
    public class VoiceChatModule : CustomBaseModule
    {
        [Command("all")]
        [Description("gets a list of all the peeps in all voice chats")]
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
        [Description("sends a list of every one in your voice chat")]
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

            //IVoiceChannel s = (Context.User as IVoiceState).VoiceChannel;
            //if (s != null)
            //{
            //    _embed.Color = Color.Teal;
            //    _embed.Description += DateTime.Now.ToString("yy/MM/dd hh:mm tt\n");
            //    _embed.Description += $"Channel: {s.Name}\n\n";
            //
            //    await foreach (var item in s.GetUsersAsync())
            //    {
            //        foreach (SocketGuildUser usr in item)
            //        {
            //            _embed.Description += $"{usr.Nickname}\n";
            //        }
            //    }
            //}
            //else
            //{
            //    _embed.Color = Color.Orange;
            //    _embed.Description += "enter a voice chat and reuse command";
            //}
            await ReplyAsync(embed: builder.Build());
        }
    }
}
