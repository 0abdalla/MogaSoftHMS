using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    [Table("ContractDetails", Schema = "dbo")]
    public class ContractDetail
    {
        public int ContractDetailId { get; set; }
        public int StaffId { get; set; }
        public int? ContractId { get; set; }
        public double BasicSalary { get; set; }
        public double? ExtraSalary { get; set; }
        public double? Transportation { get; set; }
        public double? HousingAllowance { get; set; }
        public double? MobileAllowance { get; set; }
        public double? WorkNature { get; set; }
        public double? MealAllowance { get; set; }
        public double? Other { get; set; }
        public double? GrossSalary { get; set; }
        public double? TotalSalary { get; set; }
    }
}
