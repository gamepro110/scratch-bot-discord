using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_core
{
    public interface IBot
    {
        Task Run(string token);
    }
}
