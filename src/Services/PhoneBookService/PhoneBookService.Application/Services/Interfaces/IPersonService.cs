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
    public interface IPersonService
    {

        Task<PersonDTO> CreateAsync(CreatePersonDTO createPersonDto);
        Task<PersonDTO> UpdateAsync(Guid id, UpdatePersonDTO updatePersonDto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<PersonDTO>> GetAllsAsync();
        Task<PersonDTO> GetByIdAsync(Guid id);
        Task<IEnumerable<PersonDTO>> GetAllWithContactInfosAsync();
    }
}
