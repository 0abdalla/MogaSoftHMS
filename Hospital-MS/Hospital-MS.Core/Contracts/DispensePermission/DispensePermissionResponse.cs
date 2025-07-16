using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.DispensePermission;
public class DispensePermissionResponse
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public string? DispenseTo { get; set; } // يصرف الي السيد:
    public string? Notes { get; set; }
    public int? TreasuryId { get; set; }
    public string? TreasuryName { get; set; }

    public string? CostCenterNumber { get; set; }
    public int? CostCenterId { get; set; }
    public string? AccountNumber { get; set; }
    public int? AccountId { get; set; }
    public AuditResponse Audit { get; set; } = new();
}
