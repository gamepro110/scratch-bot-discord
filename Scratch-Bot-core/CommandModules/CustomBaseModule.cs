using Discord;
using Discord.Commands;

namespace Scratch_Bot_core.Modules
{
    public abstract class CustomBaseModule : ModuleBase<SocketCommandContext>
    {
        protected async Task SendEmbed(EmbedBuilder builder)
        {
            await ReplyAsync(embed: builder.Build());
        }
    }
}
