using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Beds;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class BedService(IUnitOfWork unitOfWork) : IBedService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateBedRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Enum.TryParse<BedStatus>(request.Status, true, out var bedStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

                var bed = new Bed
                {
                    Number = request.Number,
                    RoomId = request.RoomId,
                };

                await _unitOfWork.Repository<Bed>().AddAsync(bed, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);

            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<BedResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var spec = new Bed();//BedSpecification();

            var beds = await _unitOfWork.Repository<Bed>().GetAllWithSpecAsync(spec, cancellationToken);

            var result = beds.Select(x => new BedResponse
            {
                Id = x.Id,
                Number = x.Number,
                RoomId = x.RoomId,
                Status = x.Status.ToString(),
                RoomNumber = x.Room.Number,

            }).ToList();

            return ErrorResponseModel<BedResponse>.Success(GenericErrors.GetSuccess);
        }
    }
}
