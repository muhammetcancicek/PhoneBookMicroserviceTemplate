using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ReportService.Domain.Enums.Enums;

namespace ReportService.Application.DTOs.ReportDTOs
{
    public class ReportDTO : ReportWithoutContentDTO
    {
        public string Content { get; set; }

    }

    public class ReportWithoutContentDTO
    {
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public ReportStatus Status { get; set; }
    }
}
