using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class ErrorResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public Status? ErrorCode { get; set; }
        public T? Results { get; set; }

        public static ErrorResponseModel<T> Success(Error SuccessError, T? value = default)
        {
            return new ErrorResponseModel<T>
            {
                IsSuccess = true,
                Message = SuccessError.Message,
                ErrorCode = SuccessError.Status,
                Results = value
            };
        }

        public static ErrorResponseModel<T> Failure(Error Error, T? value = default)
        {
            return new ErrorResponseModel<T>
            {
                IsSuccess = false,
                Message = Error.Message,
                ErrorCode = Error.Status,
                Results = value
            };
        }
    }
}
