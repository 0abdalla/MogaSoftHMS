using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryTransactionResponse
{
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public int TreasuryId { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal CurrentBalance { get; set; }
    public List<TransactionDetail> Transactions { get; set; } = new();
}
