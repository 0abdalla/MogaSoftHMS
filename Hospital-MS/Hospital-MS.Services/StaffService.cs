using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Services.Common;
using Hospital_MS.Core.Specifications.Staffs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class StaffService(IUnitOfWork unitOfWork, IFileService fileService, UserManager<ApplicationUser> userManager) : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (!Enum.TryParse<StaffStatus>(request.Status, true, out var staffStatus))
                    return Result.Failure(new Error("InvalidStatus", "Invalid staff Status provided.", 400));

                if (!Enum.TryParse<StaffType>(request.Type, true, out var staffType))
                    return Result.Failure(new Error("InvalidType", "Invalid staff type provided.", 400));

                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return Result.Failure(new Error("InvalidGender", "Invalid Gender provided.", 400));

                if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
                    return Result.Failure(new Error("InvalidMaritalStatus", "Invalid MaritalStatus provided.", 400));


                var staff = new Staff
                {
                    FullName = ArabicNormalizer.NormalizeArabic(request.FullName),
                    Email = request.Email,
                    Specialization = request.Specialization,
                    PhoneNumber = request.PhoneNumber,
                    HireDate = request.HireDate,
                    ClinicId = request.ClinicId,
                    DepartmentId = request.DepartmentId,
                    NationalId = request.NationalId,
                    MaritalStatus = maritalStatus,
                    Gender = gender,
                    Notes = request.Notes,
                    Address = request.Address,
                    Type = staffType,
                    Status = staffStatus,

                };

                await _unitOfWork.Repository<Staff>().AddAsync(staff, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);


                // Save Attachment files

                var AttachmentItems = new List<StaffAttachments>();
                var AttachmentURLs = new List<string>();


                foreach (var file in request.Files)
                {
                    if (file.Length > 0)
                    {
                        var fileUrl = await _fileService.UploadFileAsync(file, "staff");

                        var attachment = new StaffAttachments
                        {
                            FileUrl = fileUrl,
                            StaffId = staff.Id,
                            
                        };

                        AttachmentItems.Add(attachment);
                        AttachmentURLs.Add(fileUrl);
                    }
                }

                await _unitOfWork.Repository<StaffAttachments>().AddRangeAsync(AttachmentItems, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                // Create User

                if (request.IsAuthorized)
                {
                    if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                        return Result.Failure(new Error("InvalidUserData", "UserName and Password are required.", 400));

                    var existingUser = await _userManager.FindByNameAsync(request.UserName);

                    if (existingUser != null)
                        return Result.Failure(new Error("UserAlreadyExists", "User with this username already exists.", 400));


                    var user = new ApplicationUser
                    {
                        UserName = request.UserName,
                        IsActive = true,          
                        FirstName = request.FullName,
                        LastName = string.Empty
                    };
                   
                    await _userManager.CreateAsync(user, request.Password);
                }

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(GenericErrors<Staff>.FailedToAdd);
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
                    NationalId = s.NationalId,
                }).ToList();

                return Result.Success<IReadOnlyList<StaffResponse>>(staffResponse);
            }
            catch (Exception ex)
            {
                return Result.Failure<IReadOnlyList<StaffResponse>>(new Error("GetAllStaffError", ex.Message, 500));
            }

        }

        public async Task<Result<StaffResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var spec = new StaffSpecification(id);

            var staff = await _unitOfWork.Repository<Staff>().GetByIdWithSpecAsync(spec, cancellationToken);

            if (staff is not { })
                return Result.Failure<StaffResponse>(GenericErrors<Staff>.NotFound);

            var response = new StaffResponse
            {
                Id = staff.Id,
                FullName = staff.FullName,
                Email = staff.Email,
                Specialization = staff.Specialization,
                PhoneNumber = staff.PhoneNumber,
                HireDate = staff.HireDate,
                Status = staff.Status.ToString(),
                Type = staff.Type.ToString(),
                ClinicId = staff.ClinicId,
                DepartmentId = staff.DepartmentId,
                ClinicName = staff.Clinic?.Name,
                DepartmentName = staff.Department?.Name,
                NationalId = staff.NationalId,
                Address = staff.Address,
                Notes = staff.Notes,
                Gender = staff.Gender.ToString(),
                MaritalStatus = staff.MaritalStatus.ToString(),

                AttachmentsUrls = staff.StaffAttachments.Select(a => a.FileUrl).ToList(),
            };

            return Result.Success(response);

        }

        public async Task<Result<IReadOnlyList<StaffResponse>>> GetFilteredStaffAsync(GetStaffRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new StaffSpecification(request);

            var staffs = await _unitOfWork.Repository<Staff>().GetAllWithSpecAsync(spec, cancellationToken);

            var staffResponse = staffs.Select(staff => new StaffResponse
            {
                Id = staff.Id,
                FullName = staff.FullName,
                Email = staff.Email,
                Specialization = staff.Specialization,
                PhoneNumber = staff.PhoneNumber,
                HireDate = staff.HireDate,
                Status = staff.Status.ToString(),
                Type = staff.Type.ToString(),
                ClinicId = staff.ClinicId,
                DepartmentId = staff.DepartmentId,
                ClinicName = staff.Clinic?.Name,
                DepartmentName = staff.Department?.Name,
                NationalId = staff.NationalId,

            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<StaffResponse>>(staffResponse);
        }

        public async Task<int> GetFilteredStaffCountAsync(GetStaffRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new StaffCountSpecification(request);
            return await _unitOfWork.Repository<Staff>().GetCountAsync(spec, cancellationToken);
        }

        public async Task<Result<StaffCountsResponse>> GetStaffCountsAsync(CancellationToken cancellationToken = default)
        {
            var staffs = await _unitOfWork.Repository<Staff>().GetAllAsync(cancellationToken);

            var response = new StaffCountsResponse
            {
                AdministratorsCount = staffs.Count(s => s.Type == StaffType.Administrator),
                DoctorsCount = staffs.Count(s => s.Type == StaffType.Doctor),
                NursesCount = staffs.Count(s => s.Type == StaffType.Nurse),
                WorkersCount = staffs.Count(s => s.Type == StaffType.Worker),

            };

            return Result.Success(response);
        }
    }
}
