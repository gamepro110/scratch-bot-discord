using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace Scratch_Bot_core
{
    public class CommandHandlerService : ICommandHandler
    {
        public CommandHandlerService(CommandService _commandService, DiscordSocketClient _socketClient, IServiceProvider _provider)
        {
            commandService = _commandService;
            socketClient = _socketClient;
            provider = _provider;
        }

        public async Task InitAsync()
            => await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), provider);

        public Task<IResult>? HandleCommandAsync(SocketMessage IncommingMessage)
        {
            if (IncommingMessage is SocketUserMessage msg)
            {
                int argpos = -1;
                if (msg.HasStringPrefix(
                        Settings.CommandPrefix,
                        ref argpos,
                        StringComparison.OrdinalIgnoreCase
                        ) &&
                    !msg.Author.IsBot
                    )
                {
                    var context = new SocketCommandContext(socketClient, msg);
                    return commandService.ExecuteAsync(context, argpos, provider);
                }
            }

            return null;

        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext cmdContext, IResult result)
        {
            if (!command.IsSpecified)
            {
                await cmdContext.Channel.SendMessageAsync("invalid cmd");
                return;
            }

            if (result.IsSuccess)
            { return; }

            await cmdContext.Channel.SendMessageAsync($"ERROR: {result}");
        }

        private readonly CommandService commandService;
        private readonly DiscordSocketClient socketClient;
        private readonly IServiceProvider provider;
    }
}
