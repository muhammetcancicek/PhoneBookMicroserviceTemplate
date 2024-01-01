using PhoneBookService.Application.DTOs.ContactInfoDTOs;
using PhoneBookService.Application.DTOs.PersonDTOs;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IContactInfoRepository _contactInfoRepository;

        public PersonService(IPersonRepository personRepository, IContactInfoRepository contactInfoRepository)
        {
            _personRepository = personRepository;
            _contactInfoRepository = contactInfoRepository;
        }

        public async Task<PersonDTO> CreateAsync(CreatePersonDTO createPersonDto)
        {
            var person = new Person(Guid.NewGuid(), createPersonDto.FirstName, createPersonDto.LastName, createPersonDto.Company);

            foreach (var contactInfo in createPersonDto.ContactInfos)
            {
                person.AddContactInfo(new ContactInfo(Guid.NewGuid(), contactInfo.Type, contactInfo.Content));
            }

            await _personRepository.AddAsync(person);

            return new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Company = person.Company,
                ContactInfos = person.ContactInfos.Select(c => new ContactInfoDTO { Type = c.Type, Content = c.Content }).ToList()
            };
        }

        public async Task<PersonDTO> UpdateAsync(Guid id, UpdatePersonDTO updatePersonDto)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) throw new KeyNotFoundException("Person not found");

            person.UpdateDetails(updatePersonDto.FirstName, updatePersonDto.LastName, updatePersonDto.Company);

            foreach (var contactInfo in updatePersonDto.AddedContactInfos)
            {
                person.AddContactInfo(new ContactInfo(Guid.NewGuid(),contactInfo.Type, contactInfo.Content));
            }
            foreach (var contactInfo in updatePersonDto.UpdatedContactInfos)
            {
                await _contactInfoRepository.UpdateAsync(new ContactInfo(contactInfo.Id, contactInfo.Type, contactInfo.Content));
            }

            await _personRepository.UpdateAsync(person);

            return new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Company = person.Company,
                ContactInfos = person.ContactInfos.Select(c => new ContactInfoDTO { Type = c.Type, Content = c.Content }).ToList()
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) throw new KeyNotFoundException("Person not found");

            await _personRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PersonDTO>> GetAllsAsync()
        {
            var persons = await _personRepository.GetAllAsync();

            return persons.Select(person => new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Company = person.Company,
                ContactInfos = person.ContactInfos.Select(c => new ContactInfoDTO { Type = c.Type, Content = c.Content }).ToList()
            });
        }

        public async Task<PersonDTO> GetByIdAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) throw new KeyNotFoundException("Person not found");

            return new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Company = person.Company,
                ContactInfos = person.ContactInfos.Select(c => new ContactInfoDTO { Type = c.Type, Content = c.Content }).ToList()
            };
        }
        public async Task<PersonDTO> GetByIdWithContactInfosAsync(Guid id)
        {
            var person = await _personRepository.GetByIdWithContactInfosAsync(id);

            if (person == null) return null;

            return new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Company = person.Company,
                ContactInfos = person.ContactInfos.Select(c => new ContactInfoDTO
                {
                    Id = c.Id,
                    Type = c.Type,
                    Content = c.Content
                }).ToList()
            };
        }
        public async Task<IEnumerable<PersonDTO>> GetAllWithContactInfosAsync()
        {
            var persons = await _personRepository.GetAllWithContactInfosAsync();

            return persons.Select(person => new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Company = person.Company,
                ContactInfos = person.ContactInfos.Select(c => new ContactInfoDTO
                {
                    Id = c.Id,
                    Type = c.Type,
                    Content = c.Content
                }).ToList()
            });
        }
    }
}
