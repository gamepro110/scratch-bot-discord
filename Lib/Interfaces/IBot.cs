using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib
{
    public interface IBot
    {
        Task Run(string token);
    }
}
