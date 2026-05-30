using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;
using MongoDB.Driver;
using Vanguard.Infrastructure.Persistence.Context;


namespace Vanguard.Infrastructure.Persistence.Repositories
{
    public class PredictionRepository : IPredictionRepository
    {
        private readonly IMongoCollection<Prediction> _collection;

        public PredictionRepository(MongoDbContext context)
        {
            _collection = context.Database.GetCollection<Prediction>("vanguard_predictions");
        }

        public async Task CreateAsync(Prediction prediction)
        {
            await _collection.InsertOneAsync(prediction);
        }

        public async Task<Prediction?> GetByIdAsync(Guid id)
        {
            return await _collection
                .Find(prediction => prediction.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Prediction>> GetAllAsync()
        {
            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task UpdateAsync(Prediction prediction)
        {
            await _collection.ReplaceOneAsync(
                item => item.Id == prediction.Id,
                prediction);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.DeleteOneAsync(
                prediction => prediction.Id == id);
        }
    }
}