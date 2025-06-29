using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseRequests;
public class PurchaseRequestItemRequest
{
    public int ItemId { get; set; }
    public decimal Quantity { get; set; }
    public string? Notes { get; set; }
}
