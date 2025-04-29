using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Beds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IBedService
    {
        Task<ErrorResponseModel<string>> CreateAsync(CreateBedRequest request, CancellationToken cancellationToken = default);
        Task<ErrorResponseModel<List<BedResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
