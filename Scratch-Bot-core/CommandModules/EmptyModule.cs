using Discord;
using Discord.Commands;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Scratch_Bot_core.Modules
{
    public partial class EmptyModule : CustomBaseModule
    {
        public EmptyModule(IServiceProvider _provider)
        {
            provider = _provider;
        }

        [Command("no")]
        public async Task No()
        {
            await ReplyAsync("awh :crying_cat_face:");
        }

        [Command("wisper", true)]
        [Description("leaving a message without peeps knowing who it comes from")]
        [RequireUserPermission(ChannelPermission.ManageMessages), RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task Wisp([Remainder] string msg)
        {
            EmbedBuilder _embed = new();

            await (Context.Channel as ITextChannel).DeleteMessageAsync(Context.Message);

            await ReplyAsync(msg, embed: _embed.Description != "" ? _embed.Description != null ? _embed.Build() : null : null);
        }

        [Command("user", true)]
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

        private readonly IServiceProvider provider;
    }
}
