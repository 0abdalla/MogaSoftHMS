using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Patients
{
    public class PatientListResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Address { get; set; } = "";   
        public DateOnly? DateOfBirth { get; set; }
        public string PatientStatus { get; set; } = "";
        public string PatientGender { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Status { get; set; } = "";
        public string? NationalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = "";
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; } = "";
    }
}
