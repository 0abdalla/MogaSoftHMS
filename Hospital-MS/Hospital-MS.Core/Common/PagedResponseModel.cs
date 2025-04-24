using Hospital_MS.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class PagedResponseModel<T>
    {
        public PagedResponseModel()
        {
            Results = new List<T>();
        }

        public List<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string SearchText { get; set; }
        public List<FilterModel> FilterList { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public void Success(Error error)
        {
            IsSuccess = true;
            ErrorMessage = error.Message;
        }

        public void Failure(Error error)
        {
            IsSuccess = false;
            ErrorMessage = error.Message;
        }
    }
}
