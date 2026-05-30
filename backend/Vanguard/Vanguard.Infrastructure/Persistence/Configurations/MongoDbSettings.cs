using System;
using System.Collections.Generic;
using System.Text;

namespace Vanguard.Infrastructure.Persistence.Configurations
{
    public class MongoDbSettings
    {
        public const string SectionName = "MongoDb";

        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;
    }
}
