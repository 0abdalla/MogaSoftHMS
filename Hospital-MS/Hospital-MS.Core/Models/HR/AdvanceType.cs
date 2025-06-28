using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    [Table("AdvanceTypes", Schema = "dbo")]
    public class AdvanceType
    {
        public int AdvanceTypeId { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
    }
}
