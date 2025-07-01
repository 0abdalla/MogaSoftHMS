using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.AdditionNotifications;
public class AdditionNotificationRequest
{
    public DateOnly Date { get; set; }
    public int BankId { get; set; }
    public int AccountId { get; set; }
    public string CheckNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}
