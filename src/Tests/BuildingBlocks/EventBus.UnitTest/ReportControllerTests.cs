using EventBus.UnitTest.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReportService.Application.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest
{
    public class ReportControllerTests : IClassFixture<ReportControllerFixture>
    {
        private readonly ReportControllerFixture _fixture;

        public ReportControllerTests(ReportControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllReports_ShouldReturnListOfReports()
        {
            var reports = new List<ReportDTO> { };
            _fixture.MockReportService.Setup(service => service.GetAllReports())
                .ReturnsAsync(reports);

            var result = await _fixture.ReportController.GetAllReports();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedReports = Assert.IsType<List<ReportDTO>>(okResult.Value);
            Assert.Equal(reports.Count, returnedReports.Count);
        }

        [Fact]
        public async Task GetReportDetails_ShouldReturnReport_WhenReportExists()
        {
            var reportId = Guid.NewGuid();
            var report = new ReportDTO {  };
            _fixture.MockReportService.Setup(service => service.GetReportById(reportId))
                .ReturnsAsync(report);

            var result = await _fixture.ReportController.GetReportDetails(reportId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(report, okResult.Value);
        }

    }

}
