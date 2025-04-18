using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Enums;
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
    public class StaffService(IUnitOfWork unitOfWork) : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Enum.TryParse<StaffStatus>(request.Status, true, out var staffStatus))
                    return Result.Failure(new Error("InvalidStatus", "Invalid staff Status provided.", 400));

                if (!Enum.TryParse<StaffType>(request.Type, true, out var staffType))
                    return Result.Failure(new Error("InvalidType", "Invalid staff type provided.", 400));


                var staff = new Staff
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Specialization = request.Specialization,
                    PhoneNumber = request.PhoneNumber,
                    HireDate = request.HireDate,
                    ClinicId = request.ClinicId,
                    DepartmentId = request.DepartmentId
                };

                await _unitOfWork.Repository<Staff>().AddAsync(staff, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("CreateStaffError", ex.Message, 500));
            }

        }

        public async Task<Result<IReadOnlyList<StaffResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var staff = await _unitOfWork.Repository<Staff>().GetAllAsync(cancellationToken);

                var staffResponse = staff.Select(s => new StaffResponse
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Email = s.Email,
                    Specialization = s.Specialization,
                    PhoneNumber = s.PhoneNumber,
                    HireDate = s.HireDate,
                    Status = s.Status.ToString(),
                    Type = s.Type.ToString(),
                    ClinicId = s.ClinicId,
                    DepartmentId = s.DepartmentId,
                }).ToList();

                return Result.Success<IReadOnlyList<StaffResponse>>(staffResponse);
            }
            catch (Exception ex)
            {
                return Result.Failure<IReadOnlyList<StaffResponse>>(new Error("GetAllStaffError", ex.Message, 500));
            }

        }
    }
}
