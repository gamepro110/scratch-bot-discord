using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib.Modules
{
    public class PingRequest
    {
        private readonly Ping sender = new();
        public double totalTime = 0;
        public int plannedTrips = 0;
        public int succesfulTrips = 0;
        public double accuracy = 0;
        public double averageTime = 0;

        public async Task Send(string url, int numPing)
        {
            plannedTrips = numPing;
            for (int i = 0; i < plannedTrips; i++)
            {
                PingReply reply = await sender.SendPingAsync(url);
                if (reply.Status == IPStatus.Success)
                {
                    succesfulTrips++;
                    totalTime += reply.RoundtripTime;
                }
            }

            accuracy = (double)succesfulTrips / plannedTrips;
            averageTime = totalTime / succesfulTrips;
        }
    }
}
