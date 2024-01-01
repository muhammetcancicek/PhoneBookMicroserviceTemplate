using Microsoft.AspNetCore.Mvc;
using PhoneBookService.Application.DTOs.ContactInfoDTOs;
using PhoneBookService.Application.Services;

namespace PhoneBookService.Api.Controllers
{
    [ApiController]
    [Route("persons/{personId}/[controller]")]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService _contactInfoService;

        public ContactInfoController(IContactInfoService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contactInfo = await _contactInfoService.GetByIdAsync(id);
            if (contactInfo == null)
            {
                return NotFound();
            }
            return Ok(contactInfo);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllForPerson(Guid personId)
        {
            var contactInfos = await _contactInfoService.GetAllForPersonAsync(personId);
            return Ok(contactInfos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid personId, [FromBody] CreateContactInfoDTO createContactInfoDto)
        {
            var contactInfoId = await _contactInfoService.CreateAsync(personId, createContactInfoDto);
            return CreatedAtAction(nameof(GetById), new { id = contactInfoId }, createContactInfoDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid personId, Guid id)
        {
            await _contactInfoService.DeleteAsync(personId, id);
            return Ok();
        }
    }
}
