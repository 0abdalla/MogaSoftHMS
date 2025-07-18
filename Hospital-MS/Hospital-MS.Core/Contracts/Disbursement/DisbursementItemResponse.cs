using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Disbursement;
public class DisbursementItemResponse
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal PriceAfterTax { get; set; }
    public string? Unit { get; set; }
}
