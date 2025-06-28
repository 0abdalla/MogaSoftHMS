using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class EmployeeVacationDto
    {
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? VacationId { get; set; }
        public int VacationTypeId { get; set; }
        public string? VacationType { get; set; }
        public string? Notes { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime LastDayWork { get; set; }
        public int? Period { get; set; }
        public string? WorkflowStatusNameEN { get; set; }
        public string? WorkflowStatusNameAR { get; set; }
        public int? WorkflowStatusId { get; set; } = (int)HRWorkflowStatus.Pending; public bool IsAlternativeAvailable { get; set; }
        public int? TotalCount { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
