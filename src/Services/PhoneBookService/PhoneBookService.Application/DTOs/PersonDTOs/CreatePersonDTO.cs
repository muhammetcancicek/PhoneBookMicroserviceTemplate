using PhoneBookService.Application.DTOs.ContactInfoDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Application.DTOs.PersonDTOs
{
    public class CreatePersonDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public List<CreateContactInfoDTO> ContactInfos { get; set; }
    }
}
