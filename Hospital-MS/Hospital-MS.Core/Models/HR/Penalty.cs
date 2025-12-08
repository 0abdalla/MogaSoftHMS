using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    [Table("Penalties", Schema = "dbo")]
    public class Penalty : AuditableEntity
    {
        public int PenaltyId { get; set; }
        public int StaffId { get; set; }
        public int PenaltyTypeId { get; set; }
        public DateTime PenaltyDate { get; set; }
        public DateTime ExecutionDate { get; set; }
        public double DeductionByDays { get; set; }
        public double? DeductionAmount { get; set; }
        public double TotalDeduction { get; set; }
        public string Reason { get; set; }
        public int? WorkflowStatusId { get; set; }

    }
}
