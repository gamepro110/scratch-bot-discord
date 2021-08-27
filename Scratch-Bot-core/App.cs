namespace Scratch_Bot_core
{
    public class App : IApp
    {
        public App(IBot ibot)
        {
            bot = ibot;
        }

        public async Task Run(string token, CancellationToken cancellationToken)
        {
            await bot.Run(token, cancellationToken);
        }

        private readonly IBot bot;
    }
}
