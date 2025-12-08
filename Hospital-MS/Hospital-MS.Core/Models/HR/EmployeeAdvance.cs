using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    [Table("EmployeeAdvances", Schema = "dbo")]
    public class EmployeeAdvance : AuditableEntity
    {
        [Key]
        public int StaffAdvanceId { get; set; }
        public int AdvanceNumber { get; set; }
        public int AdvanceTypeId { get; set; }
        public string? AdvanceName { get; set; }
        public double AdvanceAmount { get; set; }
        public double PaymentAmount { get; set; }
        public double? Benefit { get; set; }
        public DateTime PaymentFromDate { get; set; }
        public DateTime PaymentToDate { get; set; }
        public int? WorkflowStatusId { get; set; }
        public string? Notes { get; set; }
        public int StaffId { get; set; }
        public Staff Staff { get; set; } = default!;
    }
}
