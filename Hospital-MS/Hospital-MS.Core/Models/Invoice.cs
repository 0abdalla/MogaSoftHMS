using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class Invoice : AuditableEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PaidAmount { get; set; }
        public BillingStatus BillingStatus { get; set; } = BillingStatus.Unpaid;

        public Patient Patient { get; set; } = default!;
        public Appointment Appointment { get; set; } = default!;
    }

}
