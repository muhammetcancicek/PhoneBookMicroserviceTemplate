using Microsoft.AspNetCore.Mvc;
using Moq;
using PhoneBookService.Api.Controllers;
using PhoneBookService.Application.DTOs.ContactInfoDTOs;
using PhoneBookService.Application.DTOs.PersonDTOs;
using PhoneBookService.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhoneBookService.Domain.Enums.Enums;

namespace EventBus.UnitTest
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _mockPersonService;
        private readonly Mock<IContactInfoService> _mockContactInfoService;
        private readonly PersonController _controller;
        Guid person1 = new Guid("b8b4bf54-20aa-4003-aeed-3edb60664e16");

        public PersonControllerTests()
        {
            _mockPersonService = new Mock<IPersonService>();
            _mockContactInfoService = new Mock<IContactInfoService>();
            _controller = new PersonController(_mockPersonService.Object, _mockContactInfoService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllPersons()
        {
            var persons = new List<PersonDTO>
        {
            new PersonDTO { Id = Guid.NewGuid(), FirstName = "Ayşe", LastName = "Yılmaz", Company = "ABC Ltd." },
            new PersonDTO { Id = Guid.NewGuid(), FirstName = "Ahmet", LastName = "Demir", Company = "XYZ A.Ş." }
        };
            _mockPersonService.Setup(service => service.GetAllWithContactInfosAsync()).ReturnsAsync(persons);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPersons = Assert.IsType<List<PersonDTO>>(okResult.Value);
            Assert.Equal(persons.Count, returnedPersons.Count);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionResult()
        {
            var createPersonDto = new CreatePersonDTO { FirstName = "Mehmet", LastName = "Kaya", Company = "DEF Şirketi" };
            var personDto = new PersonDTO { Id = Guid.NewGuid(), FirstName = "Mehmet", LastName = "Kaya", Company = "DEF Şirketi" };
            var personId = Guid.NewGuid();
            _mockPersonService.Setup(service => service.CreateAsync(createPersonDto)).ReturnsAsync(personDto);
             
            var result = await _controller.Create(createPersonDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
            Assert.Equal(createPersonDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task GetById_PersonExists_ShouldReturnPerson()
        {
            var personId = Guid.NewGuid();
            var personDto = new PersonDTO { Id = personId, FirstName = "Elif", LastName = "Çelik", Company = "GHI Kurumu" };
            _mockPersonService.Setup(service => service.GetByIdWithContactInfosAsync(personId)).ReturnsAsync(personDto);

            var result = await _controller.GetById(personId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(personDto, okResult.Value);
        }

        [Fact]
        public async Task GetById_PersonNotExists_ShouldReturnNotFound()
        {
            var personId = Guid.NewGuid();
            _mockPersonService.Setup(service => service.GetByIdWithContactInfosAsync(personId)).ReturnsAsync((PersonDTO)null);

            var result = await _controller.GetById(personId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContentResult()
        {
            var personId = Guid.NewGuid();
            var updatePersonDto = new UpdatePersonDTO { FirstName = "Can", LastName = "Özdemir", Company = "JKL Şirketi" };
            var personDto = new PersonDTO { Id = Guid.NewGuid(), FirstName = "Can", LastName = "Özdemir", Company = "JKL Şirketi" };
            _mockPersonService.Setup(service => service.UpdateAsync(personId, updatePersonDto)).ReturnsAsync(personDto);

            var result = await _controller.Update(personId, updatePersonDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContentResult()
        {

            _mockPersonService.Setup(service => service.DeleteAsync(person1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(person1);

            Assert.IsType<NoContentResult>(result);
        }



    }
}
