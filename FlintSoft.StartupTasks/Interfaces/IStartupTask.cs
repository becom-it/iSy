using System.Threading;
using System.Threading.Tasks;

namespace FlintSoft.StartupTasks.Interfaces
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
