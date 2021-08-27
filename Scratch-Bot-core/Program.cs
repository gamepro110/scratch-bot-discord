using Microsoft.Extensions.DependencyInjection;
using Scratch_Bot_core;

// configue service provider
ServiceProvider provider = (ServiceProvider)ContainerConfig.Configure();
IApp app = (IApp)provider.GetService(typeof(IApp));

if (app != null)
{
    string[] argLines = File.ReadAllLines(Settings.TokenFile);
    if (argLines.Length == 2)
    {
        Settings.WebhookUrl = argLines[0]; // set the webhook url
        await app.Run(argLines[1], provider.GetService<CancellationTokenSource>().Token);
    }
    else
    {
        Console.WriteLine($"something went wrong while reading {Settings.TokenFile}");
    }
}
else
{
    Console.WriteLine("IAPP was null...");
}
