﻿using PhoneBookService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookService.Domain.Interfaces.RepositoryInterfaces
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<IEnumerable<Person>> GetAllWithContactInfosAsync();
        Task<Person> GetByIdWithContactInfosAsync(Guid id);
    }
}
