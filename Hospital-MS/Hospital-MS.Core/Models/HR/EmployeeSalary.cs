using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    [Table("EmployeeSalaries", Schema = "finance")]
    public class EmployeeSalary: AuditableEntity
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int EmployeeCode { get; set; }
        public DateTime Date { get; set; }
        public string? VisaCode { get; set; }
        public string? Name { get; set; }
        public double BasicSalary { get; set; }
        public double DailyRate { get; set; }
        public int AttendanceDays { get; set; }
        public double Total { get; set; }
        public double Insurance { get; set; }
        public double NetSalary { get; set; }
        public double Taxes { get; set; }
        public double Advances { get; set; }
        public double AmountDue { get; set; }
    }
}
