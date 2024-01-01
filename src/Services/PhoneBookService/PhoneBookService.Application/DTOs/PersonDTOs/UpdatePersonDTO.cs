using PhoneBookService.Application.DTOs.ContactInfoDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Application.DTOs.PersonDTOs
{
    public class UpdatePersonDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public List<CreateContactInfoDTO> AddedContactInfos { get; set; }
        public List<UpdateContactInfoDTO> UpdatedContactInfos { get; set; }
    }
}
