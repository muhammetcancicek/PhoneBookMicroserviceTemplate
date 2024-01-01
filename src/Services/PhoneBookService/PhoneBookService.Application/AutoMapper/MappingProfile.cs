using AutoMapper;
using PhoneBookService.Application.DTOs.PersonDTOs;
using PhoneBookService.Domain.Entities;

namespace PhoneBookService.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>();
            CreateMap<CreatePersonDTO, Person>();
        }
    }
}
