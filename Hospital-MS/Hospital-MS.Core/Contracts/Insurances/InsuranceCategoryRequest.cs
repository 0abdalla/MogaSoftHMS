using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Insurances
{
    public class InsuranceCategoryRequest
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
    }
}
