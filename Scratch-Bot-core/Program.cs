using Microsoft.Extensions.DependencyInjection;
using Scratch_Bot_core;
using System;
using System.IO;
using System.Threading;

// configue service provider
ServiceProvider provider = (ServiceProvider)ContainerConfig.Configure();
IApp app = (IApp)provider.GetService(typeof(IApp));

if (app != null)
{
    if (File.Exists(Settings.TokenFile))
    {
        string[] argLines = File.ReadAllLines(Settings.TokenFile);
        if (argLines.Length == 2)
        {
            Settings.WebhookUrl = argLines[0]; // set the webhook url
            CancellationTokenSource? tokenSource = provider.GetService<CancellationTokenSource>();
            if (tokenSource != null)
            {
                await app.Run(argLines[1], tokenSource.Token);
            }
        }
        else
        {
            Console.WriteLine($"something went wrong while reading {Settings.TokenFile}");
        }
    }
    else
    {
        Console.WriteLine($"{Settings.TokenFile} not found");
    }
}
else
{
    Console.WriteLine("IAPP was null...");
}
