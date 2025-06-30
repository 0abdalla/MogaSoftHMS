using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Banks;
public class BankResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public string? AccountNumber { get; set; }
    public string Currency { get; set; }
    public decimal InitialBalance { get; set; }
    public bool IsActive { get; set; }

    public AuditResponse Audit { get; set; }
}