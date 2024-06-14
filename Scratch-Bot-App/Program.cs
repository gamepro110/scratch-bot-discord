using Scratch_Bot_Lib;

using Discord;
using Microsoft.Extensions.DependencyInjection;

using System.IO;

namespace Scratch_Bot_App
{
    public class Program
    {
        public static void Main() => Run();

        private static async void Run()
        {
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
                    await loggingService.Log($"{Settings.TokenFile} not found", LogSeverity.Error);
                }
            }
            else
            {
                await loggingService.Log("IAPP was null...\n", LogSeverity.Error);
            }
        }

        static Program()
        {
            app = (IApp)provider.GetService(typeof(IApp));
            loggingService = (ILogger)provider.GetService(typeof(ILogger));
        }

        private static readonly ServiceProvider provider = (ServiceProvider)ContainerConfig.Configure();
        private static readonly IApp app;
        private static readonly ILogger loggingService;
    }
}