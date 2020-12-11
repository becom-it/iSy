using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlintSoft.StartupTasks.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Becom.EDI.PersonalDataExchange.Services
{
    public class ZeiterfassungsCustomizingCacheStartupTask : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;

        public ZeiterfassungsCustomizingCacheStartupTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var zeiterfassungsService = scope.ServiceProvider.GetRequiredService<IZeiterfassungsService>();
            await zeiterfassungsService.GetZeiterfassungsCustomizing();
        }
    }
}
