using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    public class JobTitle : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public StatusTypes Status { get; set; }

        public int? JobDepartmentId { get; set; }
        public JobDepartment? JobDepartment { get; set; } = default!;

        public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();
    }
}
