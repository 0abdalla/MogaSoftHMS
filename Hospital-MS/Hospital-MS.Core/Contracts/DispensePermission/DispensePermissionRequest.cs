using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.DispensePermission;
public class DispensePermissionRequest
{
    public DateOnly Date { get; set; }
    public int TreasuryId { get; set; }
    public string? DispenseTo { get; set ; }
    public string? Notes { get; set; }
    public int AccountId { get; set; }
    public int CostCenterId { get; set; }
    public decimal Amount { get; set; }
}
