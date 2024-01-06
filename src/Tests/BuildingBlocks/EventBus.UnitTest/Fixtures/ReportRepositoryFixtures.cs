using MongoDB.Driver;
using Moq;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.UnitTest.Fixtures
{
    public class ReportRepositoryFixture
    {
        public Mock<IMongoCollection<Report>> MockCollection { get; private set; }
        public Mock<IMongoDatabase> MockDatabase { get; private set; }
        public ReportRepository ReportRepository { get; private set; }

        public ReportRepositoryFixture()
        {
            MockCollection = new Mock<IMongoCollection<Report>>();
            MockDatabase = new Mock<IMongoDatabase>();
            MockDatabase.Setup(db => db.GetCollection<Report>("Reports", null))
                        .Returns(MockCollection.Object);

            ReportRepository = new ReportRepository(MockDatabase.Object);
        }
    }
}
