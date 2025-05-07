using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    public class JobType : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public StatusTypes Status { get; set; }

        public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();
    }
}
