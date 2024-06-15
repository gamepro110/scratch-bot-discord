using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib.Modules
{
    public abstract class CustomBaseModule : ModuleBase<SocketCommandContext>
    {
        protected async Task SendEmbed(EmbedBuilder builder)
        {
            await ReplyAsync(embed: builder.Build());
        }
    }
}
