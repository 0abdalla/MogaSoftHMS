using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class MedicalServiceDetail
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int MedicalServiceId { get; set; }
        public int? RadiologyBodyTypeId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public Appointment Appointment { get; set; }
        public MedicalService? MedicalService { get; set; }
    }
}
