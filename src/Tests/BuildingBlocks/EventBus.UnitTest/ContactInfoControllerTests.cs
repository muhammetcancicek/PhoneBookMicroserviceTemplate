using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest
{
    using Xunit;
    using Moq;
    using PhoneBookService.Application.Services;
    using PhoneBookService.Api.Controllers;
    using PhoneBookService.Application.DTOs.ContactInfoDTOs;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using static PhoneBookService.Domain.Enums.Enums;
    using PhoneBookService.Domain.Entities;

    public class ContactInfoControllerTests
    {
        private readonly Mock<IContactInfoService> _mockContactInfoService;
        private readonly ContactInfoController _controller;
        Guid person1 = new Guid("b8b4bf54-20aa-4003-aeed-3edb60664e16");

        public ContactInfoControllerTests()
        {
            _mockContactInfoService = new Mock<IContactInfoService>();
            _controller = new ContactInfoController(_mockContactInfoService.Object);
        }

        [Fact]
        public async Task GetById_ContactInfoExists_ShouldReturnContactInfo()
        {
            // Arrange
            var contactInfoId = Guid.NewGuid();
            var contactInfoDto = new ContactInfoDTO { Id = contactInfoId, Type = ContactType.EmailAddress, Content = "kullanici@gmail.com" };
            _mockContactInfoService.Setup(service => service.GetByIdAsync(contactInfoId)).ReturnsAsync(contactInfoDto);

            // Act
            var result = await _controller.GetById(contactInfoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contactInfoDto, okResult.Value);
        }

        [Fact]
        public async Task GetById_ContactInfoNotExists_ShouldReturnNotFound()
        {
            var contactInfoId = Guid.NewGuid();
            _mockContactInfoService.Setup(service => service.GetByIdAsync(contactInfoId)).ReturnsAsync((ContactInfoDTO)null);

            var result = await _controller.GetById(contactInfoId);
            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllForPerson_ShouldReturnContactInfos()
        {
            var contactInfoDtos = new List<ContactInfoDTO>
        {
            new ContactInfoDTO { Id = Guid.NewGuid(), Type = ContactType.EmailAddress, Content = "kullanici@gmail.com" },
            new ContactInfoDTO { Id = Guid.NewGuid(), Type = ContactType.PhoneNumber, Content = "0544 444 44 44" }
        };
            var contactInfos = new List<ContactInfo>
        {
            new ContactInfo { Id = Guid.NewGuid(), Type = ContactType.EmailAddress, Content = "kullanici@gmail.com" },
            new ContactInfo { Id = Guid.NewGuid(), Type = ContactType.PhoneNumber, Content = "0544 444 44 44" }
        };
            _mockContactInfoService.Setup(service => service.GetAllForPersonAsync(person1)).ReturnsAsync(contactInfos);

            var result = await _controller.GetAllForPerson(person1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedContactInfos = Assert.IsType<List<ContactInfo>>(okResult.Value);
            Assert.Equal(contactInfoDtos.Count, returnedContactInfos.Count);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionResult()
        {
            var personId = Guid.NewGuid();
            var createContactInfoDto = new CreateContactInfoDTO { Type = ContactType.EmailAddress, Content = "kullanici@gmail.com" };
            var contactInfoId = Guid.NewGuid();
            _mockContactInfoService.Setup(service => service.CreateAsync(personId, createContactInfoDto)).ReturnsAsync(contactInfoId);

            var result = await _controller.Create(personId, createContactInfoDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
            Assert.Equal(createContactInfoDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOkResult()
        {
            var contactInfoId = Guid.NewGuid();
            _mockContactInfoService.Setup(service => service.DeleteAsync(contactInfoId)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(contactInfoId);

            Assert.IsType<OkResult>(result);
        }

    }

}
