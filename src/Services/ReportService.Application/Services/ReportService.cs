using ReportService.Application.Services.Interfaces;
using PhoneBookService.Messaging;
using ReportService.Domain.Entities;
using ReportService.Domain.Interfaces;
using System.Text;
using static ReportService.Domain.Enums.Enums;
using PhoneBookService.Domain.Interfaces.RepositoryInterfaces;
using ReportService.Application.DTOs.ReportDTOs;

namespace ReportService.Application.Services
{
    
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IContactInfoRepository _contactInfoRepository;
        private const string ReportRequestQueueName = "report_requests";
        private const string ReportResponseQueueName = "report_responses";


        public ReportService(IReportRepository reportRepository, IRabbitMqService rabbitMqService, IContactInfoRepository contactInfoRepository)
        {
            _reportRepository = reportRepository;
            _rabbitMqService = rabbitMqService;
            _contactInfoRepository = contactInfoRepository;
        }


        public async Task<IEnumerable<ReportWithoutContentDTO>> GetAllReports()
        {
            var reports = await _reportRepository.GetAllAsync();
            return reports.Select(r => new ReportWithoutContentDTO
            {
                Id = r.Id,
                RequestedDate = r.RequestedDate,
                Status = r.Status
            }).ToList();
        }

        public async Task<ReportDTO> GetReportById(Guid id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null) return null;

            return new ReportDTO
            {
                Id = report.Id,
                RequestedDate = report.RequestedDate,
                Content = report.Content,
                Status = report.Status
            };
        }

        public async Task<Guid> CreateReportRequest()
        {
            var report = new Report
            {
                Id = Guid.NewGuid(),
                RequestedDate = DateTime.UtcNow,
                Status = ReportStatus.Preparing,
                Content = ""
            };
            await _reportRepository.AddAsync(report);

            var correlationId = Guid.NewGuid().ToString();
            _rabbitMqService.SendRequest(ReportRequestQueueName, report.Id, ReportResponseQueueName, correlationId);

            //_rabbitMqService.PublishMessage(ReportQueueName, report.Id);
            return report.Id;
        }

        public async Task CreateReport(Guid reportId)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null)
            {
                throw new InvalidOperationException("Rapor bulunamadı.");
            }

            StringBuilder reportContentBuilder = await getReportText();

            report.Content = reportContentBuilder.ToString();
            report.Status = ReportStatus.Completed;

            await _reportRepository.UpdateAsync(report);
        }

        private async Task<StringBuilder> getReportText()
        {
            var locationContactInfos = await _contactInfoRepository.GetLocationContactInfosAsync();
            var cityCounts = locationContactInfos
                .GroupBy(info => info.Content)
                .Select(group => new { City = group.Key, Count = group.Count() })
                .ToList();

            var reportContentBuilder = new System.Text.StringBuilder();
            foreach (var cityCount in cityCounts)
            {
                reportContentBuilder.AppendLine($"{cityCount.City} : {cityCount.Count} kişi");
            }

            return reportContentBuilder;
        }


    }
}
