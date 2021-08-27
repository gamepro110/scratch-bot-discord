using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Scratch_Bot_core
{
    public interface ICommandHandler
    {
        Task InitAsync();
        Task<IResult>? HandleCommandAsync(SocketMessage IncommingMessage);
        Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext cmdContext, IResult result);
    }
}
