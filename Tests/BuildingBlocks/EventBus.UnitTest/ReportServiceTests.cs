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
            var mockReports = new List<Report> {  };
            _fixture.MockReportRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(mockReports);

            var result = await _fixture.ReportService.GetAllReports();

            Assert.NotNull(result);
            Assert.Equal(mockReports.Count, result.Count());
        }

        [Fact]
        public async Task GetReportById_ShouldReturnReport_WhenReportExists()
        {
            var reportId = Guid.NewGuid();
            var mockReport = new Report { Id = reportId, };
            _fixture.MockReportRepository.Setup(repo => repo.GetByIdAsync(reportId))
                .ReturnsAsync(mockReport);

            var result = await _fixture.ReportService.GetReportById(reportId);

            Assert.NotNull(result);
            Assert.Equal(reportId, result.Id);
        }
    }

}
