using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Banks;
public class BankStatementResponse
{
    public string BankName { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public decimal FinalBalance { get; set; }
    public List<StatementDetail> DepositDetails { get; set; } = new();
    public List<StatementDetail> WithdrawalDetails { get; set; } = new();
}
