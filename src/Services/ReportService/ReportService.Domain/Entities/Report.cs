using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ReportService.Domain.Enums.Enums;

namespace ReportService.Domain.Entities
{

    public class Report
    {
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Content { get; set; }
        public ReportStatus Status { get; set; }
    }
}
