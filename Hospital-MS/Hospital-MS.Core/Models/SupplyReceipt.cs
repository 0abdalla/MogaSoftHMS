using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class SupplyReceipt : AuditableEntity // ايصال التوريد
{
    public int Id { get; set; }
    public int TreasuryId { get; set; }
    public DateOnly Date { get; set; }
    public string? ReceivedFrom { get; set; }
    public string? AccountCode { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int? CostCenterId { get; set; }
    public bool IsActive { get; set; } = true; 

    public Treasury Treasury { get; set; } = default!;
    public CostCenter CostCenter { get; set; } = default!;
}
