using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Treasuries;
public class TreasuryResponse
{
    public int Id { get; set; }
    public string AccountCode { get; set; }
    public string Name { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public string Currency { get; set; }
    public decimal OpeningBalance { get; set; }
    public bool IsActive { get; set; }
    public AuditResponse Audit { get; set; }
}
