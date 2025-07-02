using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class AttendanceSalary : AuditableEntity
    {
        public int Id { get; set; }
        public string? Code { get; set; }           // رقم البصمة
        public string? Name { get; set; }           // الاسم
        public int? WorkHours { get; set; }         // عدد الساعات
        public double? WorkDays { get; set; }       // أيام العمل
        public int? RequiredHours { get; set; }     // الساعات المطلوبة
        public double? TotalFingerprintHours { get; set; } // إجمالي ساعات البصمة
        public double? SickDays { get; set; }       // الأيام القلبية
        public double? OtherDays { get; set; }      // أخرى
        public int? Fridays { get; set; }           // أيام الجمع
        public int? TotalDays { get; set; }         // إجمالي الأيام
        public int? Overtime { get; set; }          // الإضافي
    }
}
