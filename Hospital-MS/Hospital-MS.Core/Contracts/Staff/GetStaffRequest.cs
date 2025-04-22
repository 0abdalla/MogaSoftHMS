using Hospital_MS.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Staff
{
    public class GetStaffRequest
    {
        public string? Type { get; set; }

        private const int maxPageSize = 20;
        private int pageSize = 16;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public int PageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get { return search; }
            set => search = ArabicNormalizer.NormalizeArabic(value ?? string.Empty);

        }
    }
}
