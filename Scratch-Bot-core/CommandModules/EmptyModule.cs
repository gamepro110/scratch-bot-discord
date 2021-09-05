using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Scratch_Bot_core.Modules
{
    public partial class EmptyModule : CustomBaseModule
    {
        public EmptyModule()
        {
        }

        [Command("No")]
        public async Task No()
        {
            await ReplyAsync("awh :crying_cat_face:");
        }

        [Command("Wisper", true)]
        [Summary("leaving a message without peeps knowing who it comes from")]
        [RequireUserPermission(ChannelPermission.ManageMessages), RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task Wisp([Remainder] string msg)
        {
            EmbedBuilder _embed = new();

            await ((ITextChannel)Context.Channel).DeleteMessageAsync(Context.Message);

            await ReplyAsync(msg, embed: _embed.Description != "" ? _embed.Description != null ? _embed.Build() : null : null);
        }

        [Command("User", true)]
        [Summary("Display user info, not giving a user gives caller`s info (username + current status + rough account age)")]
        public async Task GetUserInfo(IUser? user = null)
        {
            user ??= Context.User;

            DateTime now = DateTime.Now;

            int ageYears = now.Year - user.CreatedAt.Year;
            int ageMonths = now.Month - user.CreatedAt.Date.Month;

            ageYears = now.Day < user.CreatedAt.Date.Day ? ageYears : ageYears--;
            ageMonths = now.Day < user.CreatedAt.Date.Day ? ageMonths-- : ageMonths + 12;

            EmbedBuilder builder = new()
            {
                Title = "User info",
                Color = Color.LightGrey,
                Description = 
                $"usrname: {user.Username}\nstatus: {user.Status}\nacc age: {ageYears}Y {ageMonths}m",
            };

            await SendEmbed(builder);
        }
    }
}
