using FlintSoft.StartupTasks.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace FlintSoft.StartupTasks.Extensions
{
    public static class StartupTaskWebHostExtensions
    {
        public static async Task RunWithTasksAsync(this IWebHost webHost, CancellationToken cancellationToken = default)
        {
            var startupTasks = webHost.Services.GetServices<IStartupTask>();

            foreach(var startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }

            await webHost.RunAsync(cancellationToken);
        }
    }
}
