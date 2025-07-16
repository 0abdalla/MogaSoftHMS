using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Disbursement;
public class DisbursementResponse
{
    public int Id { get; set; }
    public string Number { get; set; }
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public string Status { get; set; }
    public int? JobDepartmentId { get; set; }
    public string? JobDepartmentName { get; set; }
    public AuditResponse Audit { get; set; } = new();
    public List<DisbursementItemResponse> Items { get; set; } = new List<DisbursementItemResponse>();
}
