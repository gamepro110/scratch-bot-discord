using Discord;

using Microsoft.Extensions.DependencyInjection;

using Scratch_Bot_core;

using System.IO;

ServiceProvider provider = (ServiceProvider)ContainerConfig.Configure();
IApp app = (IApp)provider.GetService(typeof(IApp));
ILogger loggingService = (ILogger)provider.GetService(typeof(ILogger));

if (app != null)
{
    if (File.Exists(Settings.TokenFile))
    {
        string[] argLines = File.ReadAllLines(Settings.TokenFile);
        if (argLines.Length == 2)
        {
            Settings.WebhookUrl = argLines[0]; // set the webhook url
            await app.Run(argLines[1]);
        }
        else
        {
            await loggingService.Log($"something went wrong while reading {Settings.TokenFile}\n", Discord.LogSeverity.Error);
        }
    }
    else
    {
        await loggingService.Log($"{Settings.TokenFile} not found\n", LogSeverity.Error);
    }
}
else
{
    await loggingService.Log("IAPP was null...\n", LogSeverity.Error);
}
