using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Domain.Entities
{
    public class WorkerExecutionLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime StartedAt { get; set; }

        public DateTime FinishedAt { get; set; }

        public long DurationMs { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
