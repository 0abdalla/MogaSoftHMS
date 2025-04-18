using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Wards;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class WardService(IUnitOfWork unitOfWork) : IWardService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateWardRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var ward = new Ward
                {
                    Number = request.Number,
                    Capacity = request.Capacity
                };

                await _unitOfWork.Repository<Ward>().AddAsync(ward, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<Ward>.FailedToAdd);
            }
        }

        public async Task<Result<IReadOnlyList<WardResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var wards = await _unitOfWork.Repository<Ward>().GetAllAsync(cancellationToken);

            var response = wards.Select(ward => new WardResponse
            {
                Id = ward.Id,
                Number = ward.Number,
                Capacity = ward.Capacity
            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<WardResponse>>(response);
        }
    }
}
