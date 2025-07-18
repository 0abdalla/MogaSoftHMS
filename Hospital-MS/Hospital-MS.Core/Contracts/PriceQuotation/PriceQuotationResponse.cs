using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PriceQuotation;
public class PriceQuotationResponse
{
    public int Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public DateTime QuotationDate { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }

    public int PurchaseRequestId { get; set; }
    public string PurchaseRequestNumber { get; set; } 

    public List<PriceQuotationItemResponse> Items { get; set; } = [];
}
