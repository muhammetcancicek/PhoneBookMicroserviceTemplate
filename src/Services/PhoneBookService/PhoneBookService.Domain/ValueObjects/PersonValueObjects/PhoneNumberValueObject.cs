using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Domain.ValueObjects.PersonValueObjects
{
    public class PhoneNumberValueObject
    {
        public string Number { get; private set; }

        public PhoneNumberValueObject(string number)
        {
            if (!IsValid(number))
                throw new ArgumentException("Invalid phone number.");
            Number = number;
        }

        private bool IsValid(string number)
        {
            // Phone Number Check
            return true; 
        }

        public override bool Equals(object obj)
        {
            var other = obj as PhoneNumberValueObject;

            if (other == null)
                return false;

            return this.Number == other.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }
}
