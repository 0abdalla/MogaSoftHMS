using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    [Table("Vacations", Schema = "dbo")]
    public class Vacation
    {
        public int VacationId { get; set; }
        public int StaffId { get; set; }
        public int VacationTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime LastDayWork { get; set; }
        public int? Period { get; set; }
        public int? VacationMonth { get; set; }
        public int? WorkflowStatusId { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? LastJoinDate { get; set; }
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
