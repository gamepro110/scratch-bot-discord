using Discord;
using Discord.Commands;
using System.Net.NetworkInformation;

namespace Scratch_Bot_core.Modules
{
    public partial class SudoModule : CustomBaseModule
    {
        [Group("ping")]
        public class PingModule : CustomBaseModule
        {
            [Command("web")]
            public async Task webPing(string url = "google.com", int numPing = 4)
            {
                Ping pingSender = new();
                double totalTime = 0;
                int succesfulTrips = 0;

                for (int i = 0; i < numPing; i++)
                {
                    PingReply reply = await pingSender.SendPingAsync(url);
                    if (reply.Status == IPStatus.Success)
                    {
                        totalTime += reply.RoundtripTime;
                        succesfulTrips++;
                    }
                }
                string txt = "";

                txt += $"requests (send/recieved/accuracy): ({numPing}/{succesfulTrips}/{(float)succesfulTrips/ numPing})";
                txt += $"time (total/avg): ({totalTime}/{totalTime / succesfulTrips})";


                EmbedBuilder builder = new()
                {
                    Title = $"ping {url}",
                    Description = txt,
                };
                
                await SendEmbed(builder);
            }
        }
    }
}
