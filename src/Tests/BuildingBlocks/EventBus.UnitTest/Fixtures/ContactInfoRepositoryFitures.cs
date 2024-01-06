using MongoDB.Driver;
using Moq;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using PhoneBookService.Infrastructure.Repositoryes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest.Fixtures
{
    public class ContactInfoRepositoryFixture
    {
        public Mock<IMongoDatabase> MockDatabase { get; private set; }
        public Mock<IMongoCollection<ContactInfo>> MockCollection { get; private set; }
        public ContactInfoRepository Repository { get; private set; }

        public ContactInfoRepositoryFixture()
        {
            MockDatabase = new Mock<IMongoDatabase>();
            MockCollection = new Mock<IMongoCollection<ContactInfo>>();
            MockDatabase.Setup(d => d.GetCollection<ContactInfo>(It.IsAny<string>(), null))
                        .Returns(MockCollection.Object);
            Repository = new ContactInfoRepository(MockDatabase.Object);
        }
    }
}
