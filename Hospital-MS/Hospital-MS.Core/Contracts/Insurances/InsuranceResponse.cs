using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Insurances
{
    public class InsuranceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public string? Code { get; set; }
        public DateOnly ContractStartDate { get; set; }
        public DateOnly ContractEndDate { get; set; }
        public bool IsActive { get; set; }

        public List<InsuranceCategoryResponse>? InsuranceCategories { get; set; }

        // Audit Information
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
