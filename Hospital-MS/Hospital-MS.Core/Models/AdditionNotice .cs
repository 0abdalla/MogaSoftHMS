using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class AdditionNotice : AuditableEntity // اشعار اضافة
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int BankId { get; set; }
    public int AccountId { get; set; }
    public string? CheckNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;

    public Bank Bank { get; set; } = default!;
    public Account Account { get; set; } = default!;
}