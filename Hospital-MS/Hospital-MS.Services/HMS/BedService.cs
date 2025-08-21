using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Beds;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ErrorResponseModel<List<BedResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var beds = await _unitOfWork.Repository<Bed>()
                .GetAll()
                .Include(i => i.Room)
                .OrderByDescending(i => i.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            var result = beds.Select(x => new BedResponse
            {
                Id = x.Id,
                Number = x.Number,
                RoomId = x.RoomId,
                Status = x.Status.ToString(),
                RoomNumber = x.Room.Number,

            }).ToList();

            return ErrorResponseModel<List<BedResponse>>.Success(GenericErrors.GetSuccess, result);
        }
    }
}
