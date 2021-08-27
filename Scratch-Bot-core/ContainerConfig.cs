using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Scratch_Bot_core.Modules;

namespace Scratch_Bot_core
{
    public class ContainerConfig
    {
        public static IServiceProvider Configure() =>
            new ServiceCollection()
            // app
            .AddSingleton<IApp, App>()
            // bot
            .AddSingleton<IBot, Bot>()
            // logging
            .AddSingleton<ILogger, ConsoleLoggingService>()
            //.AddSingleton<ILogger, FileLoggingService>()
            //.AddSingleton<ILogger, WebhookLoggingService>()
            .AddSingleton<LoggingService>(provider => new(provider.GetServices<ILogger>()))
            // modules
            .AddSingleton<HelpModule>()
            .AddSingleton<EmptyModule>()
            .AddSingleton<SudoModule>()
            // services
            .AddSingleton<DiscordSocketClient>(provider => new(Settings.socketConfig))
            .AddSingleton<CommandService>(provider => new(Settings.commandConfig))
            .AddSingleton<ICommandHandler, CommandHandlerService>()
            // etc.
            .AddSingleton<CancellationTokenSource>()
            .BuildServiceProvider();
    }
}
