using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class PagingFilterModel
    {
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<FilterModel> FilterList { get; set; }
    }
}
