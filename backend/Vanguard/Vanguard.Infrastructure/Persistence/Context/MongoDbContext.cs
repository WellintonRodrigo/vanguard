using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Vanguard.Infrastructure.Persistence.Configurations;

namespace Vanguard.Infrastructure.Persistence.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }
        public IMongoDatabase Database => _database;
    }
}
