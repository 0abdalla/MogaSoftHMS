using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Wards;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class WardService(IUnitOfWork unitOfWork) : IWardService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateWardRequest request, CancellationToken cancellationToken = default)
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

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<List<WardResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var wards = await _unitOfWork.Repository<Ward>().GetAll().ToListAsync();

            var response = wards.Select(ward => new WardResponse
            {
                Id = ward.Id,
                Number = ward.Number,
                Capacity = ward.Capacity
            }).ToList();

            return ErrorResponseModel<List<WardResponse>>.Success(GenericErrors.GetSuccess, response);
        }
    }
}
