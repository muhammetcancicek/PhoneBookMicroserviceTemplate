using Moq;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhoneBookService.Domain.Enums.Enums;

namespace EventBus.UnitTest.Fixtures
{
    public class DatabaseFixture
    {
        public Mock<IPersonRepository> MockPersonRepository { get; private set; }
        public Mock<IContactInfoRepository> MockContactInfoRepository { get; private set; }
        Guid person1 = new Guid("b8b4bf54-20aa-4003-aeed-3edb60664e16");
        public Guid person2 = Guid.NewGuid();

        public DatabaseFixture()
        {
            MockPersonRepository = new Mock<IPersonRepository>();
            MockContactInfoRepository = new Mock<IContactInfoRepository>();

            SetupMockPersonRepository();
            SetupMockContactInfoRepository();
        }

        private void SetupMockPersonRepository()
        {
            List<Person> persons = peoples();
            List <ContactInfo> contactInfos = getContactInfoList();

            MockPersonRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(persons);

            MockPersonRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => persons.Find(p => p.Id == id));

            MockPersonRepository.Setup(repo => repo.AddAsync(It.IsAny<Person>()))
                .Callback((Person person) => persons.Add(person));

            MockPersonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Person>()))
                .Callback((Person person) =>
                {
                    var index = persons.FindIndex(p => p.Id == person.Id);
                    if (index != -1)
                        persons[index] = person;
                });

            MockPersonRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
                .Callback((Guid id) => persons.RemoveAll(p => p.Id == id));

            MockPersonRepository.Setup(repo => repo.GetAllWithContactInfosAsync())
                .ReturnsAsync(() =>
                {
                    foreach (var person in persons)
                    {
                        person.ContactInfos = contactInfos.Where(c => c.PersonId == person.Id).ToList();
                    }
                    return persons;
                });

            MockPersonRepository.Setup(repo => repo.GetByIdWithContactInfosAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) =>
                {
                    var person = persons.FirstOrDefault(p => p.Id == id);
                    if (person != null)
                    {
                        person.ContactInfos = contactInfos.Where(c => c.PersonId == person.Id).ToList();
                    }
                    return person;
                });
        }

        private List<Person> peoples()
        {
            return new List<Person>
            {
                new Person
                {
                    Id = person1,
                    FirstName = "Test Kullanıcı 1",
                    LastName = "Soyadı 1",
                    Company = "Şirket 1"
                },
                new Person
                {
                    Id = person2,
                    FirstName = "Test Kullanıcı 2",
                    LastName = "Soyadı 2",
                    Company = "Şirket 2"
                }
            };
        }

        private void SetupMockContactInfoRepository()
        {
            List<ContactInfo> contactInfos = getContactInfoList();

            MockContactInfoRepository.Setup(repo => repo.GetAllByPersonIdAsync(It.IsAny<Guid>()))
              .ReturnsAsync((Guid personId) => contactInfos.FindAll(c => c.PersonId == personId));

            MockContactInfoRepository.Setup(repo => repo.AddAsync(It.IsAny<ContactInfo>()))
                .Callback((ContactInfo contactInfo) => contactInfos.Add(contactInfo));

            MockContactInfoRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ContactInfo>()))
                .Callback((ContactInfo contactInfo) =>
                {
                    var index = contactInfos.FindIndex(c => c.Id == contactInfo.Id);
                    if (index != -1)
                        contactInfos[index] = contactInfo;
                });

            MockContactInfoRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
                .Callback((Guid id) => contactInfos.RemoveAll(c => c.Id == id));
        }

        private List<ContactInfo> getContactInfoList()
        {
            return new List<ContactInfo>
            {
                new ContactInfo
                {
                    Id = Guid.NewGuid(),
                    PersonId = person1,
                    Type = ContactType.EmailAddress,
                    Content = "ornek1@mail.com"
                },
                new ContactInfo
                {
                    Id = Guid.NewGuid(),
                    PersonId = person1,
                    Type = ContactType.PhoneNumber,
                    Content = "+90 555 555 55 55"
                },
                new ContactInfo
                {
                    Id = Guid.NewGuid(),
                    PersonId = person1,
                    Type = ContactType.Location,
                    Content = "Manisa"
                },
                                new ContactInfo
                {
                    Id = Guid.NewGuid(),
                    PersonId = person2,
                    Type = ContactType.EmailAddress,
                    Content = "ornek2@mail.com"
                },
                new ContactInfo
                {
                    Id = Guid.NewGuid(),
                    PersonId = person2,
                    Type = ContactType.PhoneNumber,
                    Content = "+90 444 44 44"
                },
                new ContactInfo
                {
                    Id = Guid.NewGuid(),
                    PersonId = person2,
                    Type = ContactType.Location,
                    Content = "izmir"
                },
            };
        }
    }
}
