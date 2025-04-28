using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Wards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services
{
    public interface IWardService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateWardRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<WardResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

    }
}
