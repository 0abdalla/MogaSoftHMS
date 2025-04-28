using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class StaffAttachments
    {
        public int Id { get; set; }
        public string FileUrl { get; set; } = string.Empty;

        public int StaffId { get; set; }
        public Staff Staff { get; set; } = default!;
    }
}
