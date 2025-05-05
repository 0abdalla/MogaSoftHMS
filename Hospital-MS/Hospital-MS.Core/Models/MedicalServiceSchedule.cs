using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class MedicalServiceSchedule
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string WeekDay { get; set; } = string.Empty;

        public int MedicalServiceId { get; set; }
        public MedicalService MedicalService { get; set; } = default!;
    }
}
