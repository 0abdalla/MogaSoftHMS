using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.PurchaseOrder;
public class CustomerResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string NameEn { get; set; }
    public string ResponsibleName { get; set; }
    public string ResponsibleName2 { get; set; }
    public string CustomerType { get; set; }
    public string Job { get; set; }
    public string Region { get; set; }
    public string Phone { get; set; }
    public string Phone2 { get; set; }
    public string Telephone { get; set; }
    public string Telephone2 { get; set; }
    public string Email { get; set; }
    public string Notes { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentResponsible { get; set; }
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; }
    public AuditResponse Audit { get; set; }
}
