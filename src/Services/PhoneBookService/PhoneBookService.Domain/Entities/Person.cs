using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static PhoneBookService.Domain.Enums.Enums;

namespace PhoneBookService.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }

        public List<ContactInfo> ContactInfos { get; set; }

        public Person()
        {
                
        }
        public Person(Guid id, string firstName, string lastName, string company)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            ContactInfos = new List<ContactInfo>();
        }

        public void AddContactInfo(ContactInfo contactInfo)
        {
            ContactInfos.Add(contactInfo);
        }
        public void UpdateDetails(string firstName, string lastName, string company)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
        }

        public void ClearContactInfos()
        {
            ContactInfos.Clear();
        }

    }
}
