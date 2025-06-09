using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.SupplyReceipts;
public class SupplyReceiptRequest
{
    public int TreasuryId { get; set; }
    public DateOnly Date { get; set; }
    public string? ReceivedFrom { get; set; }
    public string? AccountCode { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int? CostCenterId { get; set; }
}
