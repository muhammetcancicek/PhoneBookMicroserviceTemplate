﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static PhoneBookService.Domain.Enums.Enums;

namespace PhoneBookService.Application.DTOs.ContactInfoDTOs
{
    public class CreateContactInfoDTO
    {
        public ContactType Type { get; set; }
        public string Content { get; set; }
    }
}
