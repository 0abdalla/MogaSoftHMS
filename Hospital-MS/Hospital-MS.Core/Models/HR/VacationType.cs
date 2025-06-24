using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models.HR
{
    [Table("VacationTypes", Schema = "dbo")]
    public class VacationType
    {
        public int VacationTypeId { get; set; }
        public string NameEN { get; set; }
        public string NameAR { get; set; }
        public bool IsActive { get; set; }
    }
}
