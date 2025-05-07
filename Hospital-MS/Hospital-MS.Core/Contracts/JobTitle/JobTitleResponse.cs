using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.JobTitle
{
    public class JobTitleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; }

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; } = default!;

        public AuditResponse Audit { get; set; }
    }
}
