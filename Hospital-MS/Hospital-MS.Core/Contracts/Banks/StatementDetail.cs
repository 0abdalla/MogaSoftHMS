using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Banks;
public class StatementDetail
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string CheckNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}