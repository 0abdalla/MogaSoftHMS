using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models.HR;

namespace Hospital_MS.Core.Models
{
    public class DisbursementRequest : AuditableEntity
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;

        public int? JobDepartmentId { get; set; }
        public JobDepartment? JobDepartment { get; set; } = default!;    

        public ICollection<DisbursementRequestItem> Items { get; set; } = new HashSet<DisbursementRequestItem>();
    }
}