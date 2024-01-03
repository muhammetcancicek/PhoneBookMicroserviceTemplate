using Microsoft.AspNetCore.Mvc;
using PhoneBookService.Application.DTOs.PersonDTOs;
using PhoneBookService.Application.Services;

namespace PhoneBookService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IContactInfoService _contactInfoService;

        public PersonController(IPersonService personService, IContactInfoService contactInfoService)
        {
            _personService = personService;
            _contactInfoService = contactInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _personService.GetAllWithContactInfosAsync();
            return Ok(persons);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonDTO createPersonDto)
        {
            var personId = await _personService.CreateAsync(createPersonDto);
            return CreatedAtAction(nameof(GetById), new { id = personId }, createPersonDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var person = await _personService.GetByIdWithContactInfosAsync(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonDTO updatePersonDto)
        {
            await _personService.UpdateAsync(id, updatePersonDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var personContactList = await _contactInfoService.GetAllForPersonAsync(id);
            foreach (var personContact in personContactList)
            {
                await _contactInfoService.DeleteAsync(personContact.Id);
            }
            await _personService.DeleteAsync(id);

            return NoContent();
        }
    }
}
