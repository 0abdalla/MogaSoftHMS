using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Treasuries;
public class TransactionDetail
{
    public string DocumentId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReceivedFrom { get; set; } = string.Empty;
    public decimal Credit { get; set; }
    public decimal Debit { get; set; }
}
