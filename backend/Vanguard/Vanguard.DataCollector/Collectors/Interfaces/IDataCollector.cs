using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.DataCollector.Collectors.Interfaces
{
    public interface IDataCollector<T>
    {
        Task<T> CollectAsync(CancellationToken cancellationToken = default);
    }
}
