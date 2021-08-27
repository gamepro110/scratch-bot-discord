namespace Scratch_Bot_core
{
    public interface IBot
    {
        Task Run(string token, CancellationToken cancellationToken);
    }
}
