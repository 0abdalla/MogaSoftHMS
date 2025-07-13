using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital_MS.Core.Models;

namespace Hospital_MS.Interfaces.Services
{
    public interface IDisbursementRequestService
    {
        Task<DisbursementRequest> CreateRequestAsync(DisbursementRequest request);
        Task<DisbursementRequest> GetRequestByIdAsync(int id);
        Task<IEnumerable<DisbursementRequest>> GetAllRequestsAsync();
        Task<DisbursementRequest> ProcessRequestAsync(int id, string status, string processedBy);
        Task<bool> CheckStockAvailabilityAsync(string itemCode, decimal requestedQuantity);
        Task<decimal> GetAvailableStockQuantityAsync(string itemCode);
    }
}