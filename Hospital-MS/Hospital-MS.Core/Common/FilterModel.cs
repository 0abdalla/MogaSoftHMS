using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class FilterModel
    {
        public string CategoryName { get; set; }
        public string ItemId { get; set; }
        public string ItemKey { get; set; }
        public string ItemValue { get; set; }
        public bool IsChecked { get; set; } = false;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FilterType { get; set; }
        public bool IsVisible { get; set; }
        public List<FilterModel> FilterItems { get; set; }
        public int DisplayOrder { get; set; }
    }
}
