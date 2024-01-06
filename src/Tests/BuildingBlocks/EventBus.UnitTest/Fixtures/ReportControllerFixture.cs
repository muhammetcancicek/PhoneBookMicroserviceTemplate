using Moq;
using ReportService.Api.Controllers;
using ReportService.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest.Fixtures
{
    public class ReportControllerFixture
    {
        public Mock<IReportService> MockReportService { get; private set; }
        public ReportController ReportController { get; private set; }

        public ReportControllerFixture()
        {
            MockReportService = new Mock<IReportService>();
            ReportController = new ReportController(MockReportService.Object);
        }
    }
}
