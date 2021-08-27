﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Scratch_Bot_core
{
    public class Bot : IBot
    {
        public Bot(DiscordSocketClient _socketClient, CommandService _commandService, ICommandHandler _commandHadler, IServiceProvider _provider, LoggingService _loggingService)
        {
            socketClient = _socketClient;
            commandService = _commandService;
            commandHandler = _commandHadler;
            provider = _provider;
            loggingService = _loggingService;

            AcceptedCommands = new Dictionary<Task<IResult>, SocketMessage>();

            socketClient.Log += loggingService.Log;
            commandService.Log += loggingService.Log;

            socketClient.MessageReceived += HandleIncommingMessageAsync;
            commandService.CommandExecuted += commandHandler.CommandExecutedAsync;
        }

        ~Bot()
        {
            socketClient.Log -= loggingService.Log;
            commandService.Log -= loggingService.Log;

            socketClient.MessageReceived -= HandleIncommingMessageAsync;
            commandService.CommandExecuted -= commandHandler.CommandExecutedAsync;
        }

        public async Task Run(string token, CancellationToken cancellationToken)
        {
            await socketClient.LoginAsync(TokenType.Bot, token);
            await socketClient.StartAsync();

            await commandHandler.InitAsync();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (AcceptedCommands.Count > 0)
                {
                    Task<IResult> finishedTasks = await Task.WhenAny(AcceptedCommands.Keys.ToArray());

                    if (!finishedTasks.Result.IsSuccess && finishedTasks.Result.Error != CommandError.UnknownCommand)
                    {
                        await AcceptedCommands[finishedTasks].Channel.SendMessageAsync(finishedTasks.Result.ErrorReason);
                    }

                    AcceptedCommands.Remove(finishedTasks);
                }

                await Task.Delay(800, cancellationToken);
            }

            await loggingService.Log("Exiting...", LogSeverity.Info);
        }

        private Task HandleIncommingMessageAsync(SocketMessage recievedMessage)
        {
            var msgResult = commandHandler.HandleCommandAsync(recievedMessage);
            if (msgResult != null)
            { AcceptedCommands.Add(msgResult, recievedMessage); }

            return Task.CompletedTask;
        }

        private readonly DiscordSocketClient socketClient;
        private readonly CommandService commandService;
        private readonly ICommandHandler commandHandler;
        private readonly IServiceProvider provider;
        private readonly LoggingService loggingService;

        // used for processing multiple requests at once
        private readonly Dictionary<Task<IResult>, SocketMessage> AcceptedCommands;
    }
}