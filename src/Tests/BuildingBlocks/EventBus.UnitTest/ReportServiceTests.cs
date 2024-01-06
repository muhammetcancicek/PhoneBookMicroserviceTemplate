using EventBus.UnitTest.Fixtures;
using Moq;
using ReportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest
{
    public class ReportServiceTests : IClassFixture<ReportServiceFixture>
    {
        private readonly ReportServiceFixture _fixture;

        public ReportServiceTests(ReportServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllReports_ShouldReturnReports()
        {
            // Arrange
            var mockReports = new List<Report> { /* Örnek Report nesneleri */ };
            _fixture.MockReportRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(mockReports);

            // Act
            var result = await _fixture.ReportService.GetAllReports();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockReports.Count, result.Count());
        }

        [Fact]
        public async Task GetReportById_ShouldReturnReport_WhenReportExists()
        {
            // Arrange
            var reportId = Guid.NewGuid();
            var mockReport = new Report { Id = reportId, /* Diğer özellikler */ };
            _fixture.MockReportRepository.Setup(repo => repo.GetByIdAsync(reportId))
                .ReturnsAsync(mockReport);

            // Act
            var result = await _fixture.ReportService.GetReportById(reportId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportId, result.Id);
        }
    }

}
