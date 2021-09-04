using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_core
{
    public class App : IApp
    {
        public App(IBot ibot)
        {
            bot = ibot;
        }

        public async Task Run(string token)
        {
            await bot.Run(token);
        }

        private readonly IBot bot;
    }
}
