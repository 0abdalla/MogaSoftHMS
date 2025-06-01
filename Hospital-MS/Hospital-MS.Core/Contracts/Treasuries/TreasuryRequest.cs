using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryRequest
{
    public string AccountCode { get; set; }
    public string Name { get; set; }
    public int BranchId { get; set; }
    public string Currency { get; set; }
    public decimal OpeningBalance { get; set; }
}
