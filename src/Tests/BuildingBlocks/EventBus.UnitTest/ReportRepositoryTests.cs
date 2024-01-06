using EventBus.UnitTest.Fixtures;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using ReportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest
{
    public class ReportRepositoryTests : IClassFixture<ReportRepositoryFixture>
    {
        private readonly ReportRepositoryFixture _fixture;

        public ReportRepositoryTests(ReportRepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task GetAllAsync_ReturnsAllReports()
        {
            // Arrange
            var mockReports = new List<Report> { new Report(), new Report() };
            var mockCursor = new Mock<IAsyncCursor<Report>>();
            mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                      .Returns(true)
                      .Returns(false);
            mockCursor.Setup(_ => _.Current).Returns(mockReports);

            _fixture.MockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Report>>(),
                                                           It.IsAny<FindOptions<Report, Report>>(),
                                                           It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _fixture.ReportRepository.GetAllAsync();

            // Assert
            Assert.Equal(mockReports.Count, result.Count());
        }

        [Fact]
        public async Task AddAsync_AddsReport()
        {
            var report = new Report();

            await _fixture.ReportRepository.AddAsync(report);

            _fixture.MockCollection.Verify(x => x.InsertOneAsync(report, null, default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesReport()
        {
            var reportId = Guid.NewGuid();
            var report = new Report { Id = reportId };
             
            await _fixture.ReportRepository.UpdateAsync(report);

            _fixture.MockCollection.Verify(
                x => x.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<Report>>(),
                    report,
                    It.IsAny<ReplaceOptions>(),
                    default
                ),
                Times.Once
            );
        }
        [Fact]
        public async Task DeleteAsync_DeletesReport()
        {
            var reportId = Guid.NewGuid();

            await _fixture.ReportRepository.DeleteAsync(reportId);

            _fixture.MockCollection.Verify(
                x => x.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Report>>(),
                    default
                ),
                Times.Once
            );
        }

    }
}
