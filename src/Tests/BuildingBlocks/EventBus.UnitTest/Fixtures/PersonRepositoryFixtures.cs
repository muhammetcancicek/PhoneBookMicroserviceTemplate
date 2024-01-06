using MongoDB.Driver;
using Moq;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Infrastructure.Repositoryes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest.Fixtures
{
    public class PersonRepositoryFixture
    {
        public Mock<IMongoDatabase> MockDatabase { get; private set; }
        public Mock<IMongoCollection<Person>> MockCollection { get; private set; }
        public PersonRepository Repository { get; private set; }

        public PersonRepositoryFixture()
        {
            MockDatabase = new Mock<IMongoDatabase>();
            MockCollection = new Mock<IMongoCollection<Person>>();
            MockDatabase.Setup(d => d.GetCollection<Person>(It.IsAny<string>(), null))
                        .Returns(MockCollection.Object);
            Repository = new PersonRepository(MockDatabase.Object);
        }
    }
}
