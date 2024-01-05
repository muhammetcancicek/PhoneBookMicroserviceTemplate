using ReportService.Application.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportDTO> GetReportById(Guid id);
        Task<IEnumerable<ReportWithoutContentDTO>> GetAllReports();
        Task<Guid> CreateReportRequest();
        Task CreateReport(Guid reportId);
    }
}
