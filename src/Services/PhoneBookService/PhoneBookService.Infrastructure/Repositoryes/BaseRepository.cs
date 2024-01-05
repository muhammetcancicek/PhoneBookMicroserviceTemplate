using MongoDB.Driver;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;

namespace PhoneBookService.Infrastructure.Repositoryes
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<IQueryable<T>> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id",entity.Id), entity);
            // await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", entity), entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }
    }
}
