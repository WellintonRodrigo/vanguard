using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Vanguard.Infrastructure.Persistence.Configurations;

namespace Vanguard.Infrastructure.Persistence.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = settings.Value;

        var client = new MongoClient(mongoSettings.ConnectionString);

        _database = client.GetDatabase(mongoSettings.DatabaseName);
    }

    public IMongoDatabase Database => _database;
}