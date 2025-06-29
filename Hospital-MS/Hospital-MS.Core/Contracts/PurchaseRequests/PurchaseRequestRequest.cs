using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseRequests;
public class PurchaseRequestRequest
{
    public DateTime RequestDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string Purpose { get; set; } 
    public int StoreId { get; set; }
    public string? Notes { get; set; }
    public List<PurchaseRequestItemRequest> Items { get; set; } = [];
}