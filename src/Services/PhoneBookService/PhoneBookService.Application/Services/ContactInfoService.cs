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
    public class ContactInfoService : IContactInfoService
    {
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IPersonRepository _personRepository;

        public ContactInfoService(IContactInfoRepository contactInfoRepository, IPersonRepository personRepository)
        {
            _contactInfoRepository = contactInfoRepository;
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<ContactInfo>> GetAllForPersonAsync(Guid personId)
        {
            return await _contactInfoRepository.GetAllByPersonIdAsync(personId);
        }

        public async Task<Guid> CreateAsync(Guid personId, CreateContactInfoDTO createContactInfoDto)
        {
            var person = await _personRepository.GetByIdAsync(personId);
            if (person == null)
                throw new KeyNotFoundException("Person not found");

            var contactInfo = new ContactInfo
            {
                Id = Guid.NewGuid(),
                PersonId = personId,
                Content = createContactInfoDto.Content,
                Type = createContactInfoDto.Type
            };

            await _contactInfoRepository.AddAsync(contactInfo);

            return contactInfo.Id;
        }
        
        
        public async Task DeleteAsync(Guid personId, Guid contactInfoId)
        {
            var contactInfo = await _contactInfoRepository.GetByIdAsync(contactInfoId);
            if (contactInfo == null || contactInfo.PersonId != personId)
                throw new KeyNotFoundException("ContactInfo not found or does not belong to the specified person");

            await _contactInfoRepository.DeleteAsync(contactInfoId);
        }
        public async Task<ContactInfoDTO> GetByIdAsync(Guid id)
        {
            var contactInfo = await _contactInfoRepository.GetByIdAsync(id);
            if (contactInfo == null) throw new KeyNotFoundException("Contact Info not found");

            return new ContactInfoDTO
            {
                Id = contactInfo.Id,
                Content = contactInfo.Content,
                Type = contactInfo.Type,
            };
        }

    }
}
