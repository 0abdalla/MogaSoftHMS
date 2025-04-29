using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Insurances
{
    public class InsuranceRequest
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? Code { get; set; }
        public DateOnly ContractStartDate { get; set; }
        public DateOnly ContractEndDate { get; set; }

        public List<InsuranceCategoryRequest>? InsuranceCategories { get; set; } = [];
    }
}
