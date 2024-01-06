using EventBus.UnitTest.Fixtures;
using MongoDB.Driver;
using Moq;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Infrastructure.Repositoryes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest
{
    public class PersonRepositoryTests
    {
        private readonly PersonRepository _repository;
        private readonly Mock<IMongoCollection<Person>> _mockCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;

        public PersonRepositoryTests()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockCollection = new Mock<IMongoCollection<Person>>();
            _mockDatabase.Setup(d => d.GetCollection<Person>(It.IsAny<string>(), null))
                         .Returns(_mockCollection.Object);
            _repository = new PersonRepository(_mockDatabase.Object);
        }
        [Fact]
        public async Task AddAsync_AddsPerson()
        {
            var person = new Person { Id = Guid.NewGuid(), Company = "Test company" };
            _mockCollection.Setup(x => x.InsertOneAsync(person, null, default))
                           .Verifiable();

            await _repository.AddAsync(person);

            _mockCollection.Verify();
        }
        [Fact]
        public async Task UpdateAsync_UpdatesPerson()
        {
            var person = new Person { Id = Guid.NewGuid() };

            await _repository.UpdateAsync(person);

            _mockCollection.Verify(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<Person>>(),
                                                          person,
                                                          It.IsAny<ReplaceOptions>(),
                                                          It.IsAny<CancellationToken>()),
                                   Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesPerson()
        {
            var personId = Guid.NewGuid();

            await _repository.DeleteAsync(personId);

            _mockCollection.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<Person>>(),
                                                         It.IsAny<CancellationToken>()),
                                   Times.Once);
        }
    }
}
