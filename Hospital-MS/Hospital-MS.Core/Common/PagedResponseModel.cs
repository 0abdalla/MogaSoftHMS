using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Enums;
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
        public int TotalCount { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public Status? ErrorCode { get; set; }
        public static PagedResponseModel<T> Success(Error error, int totalCount, List<T?> value = default)
        {
            return new PagedResponseModel<T>
            {
                IsSuccess = true,
                ErrorMessage = error.Message,
                ErrorCode = error.Status,
                TotalCount = totalCount,
                Results = value
            };
        }

        public static PagedResponseModel<T> Failure(Error error)
        {
            return new PagedResponseModel<T>
            {
                IsSuccess = false,
                ErrorMessage = error.Message,
                ErrorCode = error.Status,
                TotalCount = 0,
                Results = new List<T>()
            };
        }
    }
}
