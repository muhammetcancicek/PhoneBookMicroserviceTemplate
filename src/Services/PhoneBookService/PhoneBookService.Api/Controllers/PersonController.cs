using Microsoft.AspNetCore.Mvc;
using PhoneBookService.Application.DTOs.PersonDTOs;
using PhoneBookService.Application.Services;

namespace PhoneBookService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
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
            await _personService.DeleteAsync(id);
            return NoContent();
        }
    }
}
