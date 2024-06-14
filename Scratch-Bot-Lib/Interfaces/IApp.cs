using System.Threading;
using System.Threading.Tasks;

namespace Scratch_Bot_Lib
{
    public interface IApp
    {
        Task Run(string token);
    }
}
