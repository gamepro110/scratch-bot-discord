namespace Scratch_Bot_core
{
    public interface IApp
    {
        Task Run(string token, CancellationToken cancellationToken);
    }
}
