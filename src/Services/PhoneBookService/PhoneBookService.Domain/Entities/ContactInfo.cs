using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static PhoneBookService.Domain.Enums.Enums;

namespace PhoneBookService.Domain.Entities
{
    public class ContactInfo : BaseEntity
    {
        public Guid PersonId { get; set; }
        public ContactType Type { get; set; }
        public string Content { get; set; }

        public ContactInfo(Guid id, Guid personId, ContactType type, string content)
        {
            Id = id;
            Type = type;
            PersonId = personId;
            Content = content;
        }
        public ContactInfo()
        {
        }
    }
}
