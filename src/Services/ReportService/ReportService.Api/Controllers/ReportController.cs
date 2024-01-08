using Microsoft.AspNetCore.Mvc;
using PhoneBookService.Application.DTOs.ContactInfoDTOs;
using PhoneBookService.Domain.Entities;
using ReportService.Application.Services.Interfaces;
using ReportService.Domain.Entities;

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

        [HttpPost("CreateReportRequest")]
        public async Task<IActionResult> CreateReportRequest()
        {
            var reportId = await _reportService.CreateReportRequest();
            return Ok($"Rapor talebiniz alındı. Rapor ID: {reportId}");
        }
        [HttpPost("CreateReport/{id}")]
        public async Task<IActionResult> CreateReport(Guid id)
        {
            try
            {
                await _reportService.CreateReport(id);
                return CreatedAtAction(nameof(GetReportDetails), new { id = id }, null);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
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
