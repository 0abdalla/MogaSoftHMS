using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PriceQuotation;
public class PriceQuotationRequest
{
    public DateTime QuotationDate { get; set; }
    public int SupplierId { get; set; }
    public int PurchaseRequestId { get; set; }
    public string? Notes { get; set; }
    public List<PriceQuotationItemRequest> Items { get; set; } = [];
}
