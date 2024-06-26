﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib
{
    public class Bot : IBot
    {
        public Bot(DiscordSocketClient _socketClient, CommandService _commandService, ICommandHandler _commandHadler, CancellationTokenSource _cancellationTokenSource, LoggingService _loggingService)
        {
            socketClient = _socketClient;
            commandService = _commandService;
            commandHandler = _commandHadler;
            tokenSource = _cancellationTokenSource;
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

        public async Task Run(string token)
        {
            CancellationToken cancellationToken = tokenSource.Token;

            await socketClient.LoginAsync(TokenType.Bot, token);
            await socketClient.StartAsync();

            await commandHandler.InitAsync();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (AcceptedCommands.Count > 0)
                {
                    Task<IResult> finishedTasks = await Task.WhenAny(AcceptedCommands.Keys);

                    if (!finishedTasks.Result.IsSuccess && finishedTasks.Result.Error != CommandError.UnknownCommand)
                    {
                        await AcceptedCommands[finishedTasks].Channel.SendMessageAsync(finishedTasks.Result.ErrorReason);
                    }

                    AcceptedCommands.Remove(finishedTasks);
                }

                await Task.Delay(800);
            }

            await loggingService.Log("Exiting...\n", LogSeverity.Info);
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
        private readonly LoggingService loggingService;
        private readonly CancellationTokenSource tokenSource;

        // used for processing multiple requests at once
        private readonly Dictionary<Task<IResult>, SocketMessage> AcceptedCommands;
    }
}
