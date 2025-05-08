using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Common
{
    public class AuditResponse
    {
        public string CreatedBy { get; set; } 
        public DateTime CreatedOn { get; set; } 

        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
