using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class StaffService(IUnitOfWork unitOfWork, IFileService fileService) : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;

        public async Task<ErrorResponseModel<string>> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (!Enum.TryParse<StaffStatus>(request.Status, true, out var staffStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

                if (!Enum.TryParse<StaffType>(request.Type, true, out var staffType))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidType);

                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidGender);

                if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidMaritalStatus);


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

                await transaction.CommitAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        //public async Task<ErrorResponseModel<StaffResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var staff = await _unitOfWork.Repository<Staff>().GetAllAsync(cancellationToken);

        //        var staffResponse = staff.Select(s => new StaffResponse
        //        {
        //            Id = s.Id,
        //            FullName = s.FullName,
        //            Email = s.Email,
        //            Specialization = s.Specialization,
        //            PhoneNumber = s.PhoneNumber,
        //            HireDate = s.HireDate,
        //            Status = s.Status.ToString(),
        //            Type = s.Type.ToString(),
        //            ClinicId = s.ClinicId,
        //            DepartmentId = s.DepartmentId,
        //            NationalId = s.NationalId,
        //        }).ToList();

        //        return ErrorResponseModel<StaffResponse>.Success(GenericErrors.GetSuccess);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponseModel<StaffResponse>.Failure(GenericErrors.TransFailed);
        //    }

        //}

        public async Task<ErrorResponseModel<StaffResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var staff = await _unitOfWork.Repository<Staff>().GetAll(i => i.Id == id).Include(x => x.Clinic).Include(x => x.Department).Include(x => x.StaffAttachments).FirstOrDefaultAsync();

            if (staff is not { })
                return ErrorResponseModel<StaffResponse>.Failure(GenericErrors.NotFound);

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

            return ErrorResponseModel<StaffResponse>.Success(GenericErrors.GetSuccess, response);

        }

        //public async Task<ErrorResponseModel<StaffResponse>> GetFilteredStaffAsync(GetStaffRequest request, CancellationToken cancellationToken = default)
        //{
        //    var spec = new Staff();//StaffSpecification(request);

        //    var staffs = await _unitOfWork.Repository<Staff>().GetAllWithSpecAsync(spec, cancellationToken);

        //    var staffResponse = staffs.Select(staff => new StaffResponse
        //    {
        //        Id = staff.Id,
        //        FullName = staff.FullName,
        //        Email = staff.Email,
        //        Specialization = staff.Specialization,
        //        PhoneNumber = staff.PhoneNumber,
        //        HireDate = staff.HireDate,
        //        Status = staff.Status.ToString(),
        //        Type = staff.Type.ToString(),
        //        ClinicId = staff.ClinicId,
        //        DepartmentId = staff.DepartmentId,
        //        ClinicName = staff.Clinic?.Name,
        //        DepartmentName = staff.Department?.Name,
        //        NationalId = staff.NationalId,

        //    }).ToList().AsReadOnly();

        //    return ErrorResponseModel<StaffResponse>.Success(GenericErrors.GetSuccess);
        //}

        //public async Task<int> GetFilteredStaffCountAsync(GetStaffRequest request, CancellationToken cancellationToken = default)
        //{
        //    var spec = new Staff();//StaffCountSpecification(request);
        //    return await _unitOfWork.Repository<Staff>().GetCountAsync(spec, cancellationToken);
        //}

        //public async Task<ErrorResponseModel<StaffCountsResponse>> GetStaffCountsAsync(CancellationToken cancellationToken = default)
        //{
        //    var staffs = await _unitOfWork.Repository<Staff>().GetAllAsync(cancellationToken);

        //    var response = new StaffCountsResponse
        //    {
        //        AdministratorsCount = staffs.Count(s => s.Type == StaffType.Administrator),
        //        DoctorsCount = staffs.Count(s => s.Type == StaffType.Doctor),
        //        NursesCount = staffs.Count(s => s.Type == StaffType.Nurse),
        //        WorkersCount = staffs.Count(s => s.Type == StaffType.Worker),

        //    };

        //    return ErrorResponseModel<StaffCountsResponse>.Success(GenericErrors.GetSuccess, response);
        //}
    }
}
