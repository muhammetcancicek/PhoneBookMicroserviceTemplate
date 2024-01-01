using PhoneBookService.Application.DTOs.PersonDTOs;
using PhoneBookService.Domain.Entities;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Application.Services
{
    public class MappingService
    {
        private readonly IMapper _mapper;

        public MappingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PersonDTO MapToPersonDTO(Person person)
        {
            return _mapper.Map<PersonDTO>(person);
        }

    }
}
