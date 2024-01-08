using Moq;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using ReportService.Domain.Interfaces;

namespace EventBus.UnitTest.Fixtures
{
    public class ReportServiceFixture
    {
        public Mock<IReportRepository> MockReportRepository { get; private set; }
        public Mock<IContactInfoRepository> MockContactInfoRepository { get; private set; }
        public ReportService.Application.Services.ReportService ReportService { get; private set; }

        public ReportServiceFixture()
        {
            MockReportRepository = new Mock<IReportRepository>();
            MockContactInfoRepository = new Mock<IContactInfoRepository>();
            ReportService = new ReportService.Application.Services.ReportService(MockReportRepository.Object, MockContactInfoRepository.Object);
        }
    }
}
