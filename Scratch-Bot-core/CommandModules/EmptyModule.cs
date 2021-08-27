using Discord;
using Discord.Commands;

namespace Scratch_Bot_core.Modules
{
    public class EmptyModule : CustomBaseModule
    {
        public EmptyModule(IServiceProvider _provider)
        {
            provider = _provider;
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
