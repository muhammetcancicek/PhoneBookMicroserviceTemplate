using MongoDB.Driver;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;

namespace PhoneBookService.Infrastructure.Repositoryes
{
    public class ContactInfoRepository : BaseRepository<ContactInfo> ,IContactInfoRepository
    {
        public ContactInfoRepository(IMongoDatabase database) : base(database, "ContactInfos")
        {
        }

        public async Task<IEnumerable<ContactInfo>> GetAllByPersonIdAsync(Guid personId)
        {
            return await _collection.Find(c => c.PersonId == personId).ToListAsync();
        }

    }
}
