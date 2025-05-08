using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.JobTitle
{
    public class JobTitleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; }

        public int? JobDepartmentId { get; set; }
    }
}
