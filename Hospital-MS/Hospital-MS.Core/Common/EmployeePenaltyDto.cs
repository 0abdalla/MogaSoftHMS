using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class EmployeePenaltyDto
    {
        public int? PenaltyId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? PenaltyTypeId { get; set; }
        public string? PenaltyType { get; set; }
        public DateTime? PenaltyDate { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public double? DeductionByDays { get; set; }
        public double? DeductionAmount { get; set; }
        public double? TotalDeduction { get; set; }
        public string? Reason { get; set; }
        public string? WorkflowStatusNameEN { get; set; }
        public string? WorkflowStatusNameAR { get; set; }
        public int? WorkflowStatusId { get; set; } = (int)HRWorkflowStatus.Pending; public int? TotalCount { get; set; }
        public string? BranchName { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
