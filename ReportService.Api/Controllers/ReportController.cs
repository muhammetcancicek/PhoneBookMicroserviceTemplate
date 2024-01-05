using Microsoft.AspNetCore.Mvc;
using ReportService.Application.Services.Interfaces;

namespace ReportService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReportRequest()
        {
            var reportId = await _reportService.CreateReportRequest();
            return Ok($"Rapor talebiniz alındı. Rapor ID: {reportId}");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReports();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportDetails(Guid id)
        {
            var report = await _reportService.GetReportById(id);
            if (report == null)
            {
                return NotFound("Rapor bulunamadı.");
            }
            return Ok(report);
        }
    }
}
