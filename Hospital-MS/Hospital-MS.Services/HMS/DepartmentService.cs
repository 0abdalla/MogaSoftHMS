﻿using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Departments;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS
{
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = new Department
                {
                    Name = request.Name,
                };

                await _unitOfWork.Repository<Department>().AddAsync(department, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);

            }
        }

        public async Task<ErrorResponseModel<List<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _unitOfWork.Repository<Department>().GetAll().ToListAsync(cancellationToken: cancellationToken);

            var response = departments.Select(d => new DepartmentResponse
            {
                Id = d.Id,
                Name = d.Name,
                
            }).ToList();

            return ErrorResponseModel<List<DepartmentResponse>>.Success(GenericErrors.GetSuccess, response);
        }
    }
}
