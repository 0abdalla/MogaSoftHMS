using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class MedicalServiceDetail : AuditableEntity
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int MedicalServiceId { get; set; }
        public int? RadiologyBodyTypeId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Appointment Appointment { get; set; }
        public MedicalService? MedicalService { get; set; }
    }
}
