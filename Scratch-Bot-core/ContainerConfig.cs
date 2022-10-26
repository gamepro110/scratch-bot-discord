using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

using Scratch_Bot_core.Modules;
using Scratch_Bot_core.TypeReaders;
using System;
using System.Threading;

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
            //.AddSingleton<ILogger, ConsoleLoggingService>()
            .AddSingleton<ILogger, FileLoggingService>()
            //.AddSingleton<ILogger, WebhookLoggingService>()
            .AddSingleton<LoggingService>(provider => new(provider.GetServices<ILogger>()))
            // TypeReaders
            .AddSingleton<DiceTypeReader>()
            // modules
            .AddSingleton<EmptyModule>()
            .AddSingleton<HelpModule>()
            .AddSingleton<SudoModule>()
            .AddSingleton<SudoModule.PingModule>()
            .AddSingleton<VoiceChatModule>()
            // services
            .AddSingleton<DiscordSocketClient>(provider => new(Settings.socketConfig))
            .AddSingleton<CommandService>(provider => new(Settings.commandConfig))
            .AddSingleton<ICommandHandler, CommandHandlerService>()
            // etc.
            .AddSingleton<CancellationTokenSource>()
            .AddScoped<Random>(provider => new(DateTime.UnixEpoch.GetHashCode()))
            .BuildServiceProvider();
    }
}
