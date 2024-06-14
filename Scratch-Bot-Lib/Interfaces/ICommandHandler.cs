using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib
{
    public interface ICommandHandler
    {
        Task InitAsync();
        Task<IResult>? HandleCommandAsync(SocketMessage IncommingMessage);
        Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext cmdContext, IResult result);
    }
}
