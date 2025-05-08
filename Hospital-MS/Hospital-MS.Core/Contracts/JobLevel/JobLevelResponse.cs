using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.JobLevel
{
    public class JobLevelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? Description { get; set; } 
        public string Status { get; set; }

        public AuditResponse Audit { get; set; }
    }
}
