using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class DisbursementRequestItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int DisbursementRequestId { get; set; }
    public DisbursementRequest DisbursementRequest { get; set; } = default!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = default!;
}