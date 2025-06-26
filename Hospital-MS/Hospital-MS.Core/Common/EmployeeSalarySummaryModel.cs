using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class EmployeeSalarySummaryModel
    {
        public int? TotalCount { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }
        public string JobTitle { get; set; }
        public string EmployeeNameEN { get; set; }
        public string EmployeeNameAR { get; set; }
        public int? BranchId { get; set; }
        public string BranchNameEN { get; set; }
        public string BranchNameAR { get; set; }
        public string JobNameEN { get; set; }
        public string JobNameAR { get; set; }
        public double? Deductions { get; set; }
        public double? Advances { get; set; }
        public double? Penalties { get; set; }
        public double? Overtime { get; set; }
        public double? GrossSalary { get; set; }
        public double? TotalSalary { get; set; }
        public double? BasicSalary { get; set; }
        public double? ExtraSalary { get; set; }
        public double? Transportation { get; set; }
        public double? HousingAllowance { get; set; }
        public double? MobileAllowance { get; set; }
        public double? WorkNature { get; set; }
        public double? MealAllowance { get; set; }
        public double? Other { get; set; }
        public decimal? DailySalary { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? CalculatedSalary { get; set; }
        public decimal? NetSalary { get; set; }
        public int? PresentDays { get; set; }
        public int? OffDays { get; set; }
        public int? SickDays { get; set; }
        public int? AbsentDays { get; set; }
        public int? TotalWorkingDays { get; set; }
    }
}
