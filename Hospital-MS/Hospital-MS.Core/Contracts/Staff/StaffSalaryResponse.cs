using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Staff
{
    public class StaffSalaryResponse
    {
        public string Name { get; set; }                      // الاسم
        public DateTime Date { get; set; }                    // التاريخ
        public double BasicSalary { get; set; }              // المرتب الأساسي
        public double SecondShift { get; set; }              // وردية ثانية
        public double Overtime { get; set; }                 // إضافي
        public double Total { get; set; }                    // الإجمالي
        public double Insurance { get; set; }               // تأمينات
        public double DifferenceBasicDays { get; set; }      // فرق أيام أساسي
        public double Absence { get; set; }                  // غياب
        public double TotalDeductions { get; set; }          // إجمالي الخصومات
        public double Net { get; set; }                      // الصافي
        public double Taxes { get; set; }                    // الضرائب
        public double? Penalties { get; set; }                // الجزاءات
        public double? Loans { get; set; }                    // السلف
        public double? Vacation { get; set; }                    // الأجازات الغير مدفوعة
        public double TotalDays { get; set; }                // إجمالي الأيام
        public double Due { get; set; }                      // المستحق
    }
}
