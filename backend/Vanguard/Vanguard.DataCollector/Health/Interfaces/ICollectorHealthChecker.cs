using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.DataCollector.Models;

namespace Vanguard.DataCollector.Health.Interfaces
{
    public interface ICollectorHealthChecker
    {
        Task<IReadOnlyCollection<CollectorHealthResult>> CheckAsync(
       CancellationToken cancellationToken = default);
    }
}
