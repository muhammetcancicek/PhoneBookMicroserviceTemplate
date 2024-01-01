using MongoDB.Driver;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Infrastructure
{
    public class DataSeeder
    {
        public static void SeedData(IMongoDatabase database)
        {
            var personCollection = database.GetCollection<Person>("Persons");
            var contactInfoCollection = database.GetCollection<ContactInfo>("ContactInfos");

            if (personCollection.CountDocuments(_ => true) == 0)
            {
                var persons = new List<Person>();
                for (int i = 1; i <= 10; i++)
                {
                    var person = new Person
                    {
                        Id = Guid.NewGuid(),
                        FirstName = $"Kullanıcı{i}",
                        LastName = $"Soyad{i}",
                        Company = $"Şirket{i}"
                    };
                    persons.Add(person);

                    contactInfoCollection.InsertMany(new[]
                    {
                    new ContactInfo { Id = Guid.NewGuid(), PersonId = person.Id, Type = Enums.ContactType.EmailAddress, Content = $"kullanici{i}@mail.com" },
                    new ContactInfo { Id = Guid.NewGuid(), PersonId = person.Id, Type = Enums.ContactType.PhoneNumber, Content = $"+90 5XX XXX XX {i}" }
                });
                }

                personCollection.InsertMany(persons);
            }
        }
    }
}
