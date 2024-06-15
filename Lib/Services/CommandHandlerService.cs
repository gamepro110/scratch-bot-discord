using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Scratch_Bot_Lib.Modules;
using Scratch_Bot_Lib.TypeReaders;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib
{
    public class CommandHandlerService(CommandService _commandService, DiscordSocketClient _socketClient, IServiceProvider _provider) : ICommandHandler
    {
        public async Task InitAsync()
        {
            commandService.AddTypeReader<DiceType>(provider.GetService(typeof(DiceTypeReader)) as TypeReader);
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
        }

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

        private readonly CommandService commandService = _commandService;
        private readonly DiscordSocketClient socketClient = _socketClient;
        private readonly IServiceProvider provider = _provider;
    }
}
