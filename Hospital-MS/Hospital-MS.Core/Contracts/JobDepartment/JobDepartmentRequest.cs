using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.JobDepartment
{
    public class JobDepartmentRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } 
    }
}
