using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Worker.Configuration
{
    public class WorkerSettings
    {
        public const string SectionName = "Worker";

        public int IntervalMinutes { get; set; } = 30;
    }
}
