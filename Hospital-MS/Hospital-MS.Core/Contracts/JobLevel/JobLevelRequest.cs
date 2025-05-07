using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.JobLevel
{
    public class JobLevelRequest
    {
        public string Name { get; set; } 
        public string? Description { get; set; }
        public string Status { get; set; }
    }
}
