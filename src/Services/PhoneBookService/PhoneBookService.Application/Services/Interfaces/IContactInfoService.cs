using PhoneBookService.Application.DTOs.ContactInfoDTOs;
using PhoneBookService.Application.DTOs.PersonDTOs;
using PhoneBookService.Domain.Entities;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Application.Services.Interfaces
{
    public interface IContactInfoService
    {
        Task<IEnumerable<ContactInfo>> GetAllForPersonAsync(Guid personId);
        Task<Guid> CreateAsync(Guid personId, CreateContactInfoDTO createContactInfoDto);
        Task<ContactInfoDTO> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid personId, Guid contactInfoId);
        Task DeleteAsync(Guid id);

    }
}
