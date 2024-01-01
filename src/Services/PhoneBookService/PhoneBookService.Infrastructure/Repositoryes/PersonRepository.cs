using MongoDB.Driver;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Infrastructure.Repositoryes
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(IMongoDatabase database) : base(database, "Persons")
        {
        }
        public async Task<IEnumerable<Person>> GetAllWithContactInfosAsync()
        {
            var aggregate = _collection.Aggregate()
                .Lookup("ContactInfos", "_id", "PersonId", "ContactInfos")
                .As<Person>();

            return await aggregate.ToListAsync();
        }
        public async Task<Person> GetByIdWithContactInfosAsync(Guid id)
        {
            var aggregate = _collection.Aggregate()
                .Match(p => p.Id == id)
                .Lookup("ContactInfos", "_id", "PersonId", "ContactInfos")
                .As<Person>()
                .FirstOrDefaultAsync();

            return await aggregate;
        }
    }

}
