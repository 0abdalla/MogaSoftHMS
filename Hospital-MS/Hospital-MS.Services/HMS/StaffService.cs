using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Staff;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Extensions;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class StaffService(IUnitOfWork unitOfWork, IFileService fileService, ISQLHelper sQLHelper) : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

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

        public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[6];

                var Status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;

                var FromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;

                var ToDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;

                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);

                Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);

                Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);

                Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);

                Params[4] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);

                Params[5] = new SqlParameter("@PageSize", pagingFilter.PageSize);

                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllStaff", Params);

                int totalCount = 0;

                if (dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);
                }

                //Covert Enm to Arabic 
                foreach (DataRow row in dt.Rows)
                {
                    row.TryTranslateEnum<StaffType>("Type");
                    row.TryTranslateEnum<Gender>("Gender");
                    row.TryTranslateEnum<MaritalStatus>("MaritalStatus");
                    row.TryTranslateEnum<StaffStatus>("Status");
                }

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        //
        //public async Task<ErrorResponseModel<StaffResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var staff = await _unitOfWork.Repository<Staff>().GetAllAsync(cancellationToken);

        //        var staffResponse = staff.Select(s => new StaffResponse
        //        {
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
            var staff = await _unitOfWork.Repository<Staff>().GetAll(i => i.Id == id).Include(x => x.Clinic).Include(x => x.Department).Include(x => x.StaffAttachments).FirstOrDefaultAsync(cancellationToken: cancellationToken);

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

        public async Task<PagedResponseModel<DataTable>> GetCountsAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[4];
                var Status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;
                var FromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;
                var ToDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@Status", Status ?? (object)DBNull.Value);
                Params[2] = new SqlParameter("@FromDate", FromDate ?? (object)DBNull.Value);
                Params[3] = new SqlParameter("@ToDate", ToDate ?? (object)DBNull.Value);
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetStaffTypeCountStatistics", Params);

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, 6, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> GetStaffCountsAsync(CancellationToken cancellationToken = default)
        {
            var staffs = await _unitOfWork.Repository<Staff>().GetAll().ToListAsync();

            var response = new
            {
                AdministratorsCount = staffs.Count(s => s.Type == StaffType.Administrator),
                DoctorsCount = staffs.Count(s => s.Type == StaffType.Doctor),
                NursesCount = staffs.Count(s => s.Type == StaffType.Nurse),
                WorkersCount = staffs.Count(s => s.Type == StaffType.Worker),
            };

            return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
        }
    }
}
