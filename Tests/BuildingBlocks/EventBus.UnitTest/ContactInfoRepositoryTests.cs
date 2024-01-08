using MongoDB.Driver;
using Moq;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Infrastructure.Repositoryes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EventBus.UnitTest
{
    public class ContactInfoRepositoryTests
    {
        private readonly ContactInfoRepository _repository;
        private readonly Mock<IMongoCollection<ContactInfo>> _mockCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;

        public ContactInfoRepositoryTests()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockCollection = new Mock<IMongoCollection<ContactInfo>>();
            _mockDatabase.Setup(d => d.GetCollection<ContactInfo>(It.IsAny<string>(), null))
                         .Returns(_mockCollection.Object);
            _repository = new ContactInfoRepository(_mockDatabase.Object);
        }

        [Fact]
        public async Task AddAsync_AddsContactInfo()
        {
            var contactInfo = new ContactInfo { Id = Guid.NewGuid(), Type = PhoneBookService.Domain.Enums.Enums.ContactType.PhoneNumber, Content = "0546 738 28 46" };
            _mockCollection.Setup(x => x.InsertOneAsync(contactInfo, null, default))
                           .Verifiable();

            await _repository.AddAsync(contactInfo);

            _mockCollection.Verify();
        }

        [Fact]
        public async Task UpdateAsync_UpdatesContactInfo()
        {
            var contactInfo = new ContactInfo { Id = Guid.NewGuid() };

            await _repository.UpdateAsync(contactInfo);

            _mockCollection.Verify(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<ContactInfo>>(),
                                                          contactInfo,
                                                          It.IsAny<ReplaceOptions>(),
                                                          It.IsAny<CancellationToken>()),
                                   Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesContactInfo()
        {
            var contactInfoId = Guid.NewGuid();

            await _repository.DeleteAsync(contactInfoId);

            _mockCollection.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<ContactInfo>>(),
                                                         It.IsAny<CancellationToken>()),
                                   Times.Once);
        }
    }
}