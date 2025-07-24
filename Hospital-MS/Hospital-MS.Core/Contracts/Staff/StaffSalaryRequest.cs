using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Staff
{
    public class StaffSalaryRequest
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public double Overtime { get; set; }
        public int TotalDays { get; set; }
        public DateTime Date { get; set; }
    }
}
