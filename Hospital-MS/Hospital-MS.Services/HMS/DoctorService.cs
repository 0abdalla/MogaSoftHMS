using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Doctors;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Extensions;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class DoctorService(IUnitOfWork unitOfWork, IFileService fileService, IWebHostEnvironment webHostEnvironment, ISQLHelper sQLHelper) : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateAsync(DoctorRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidGender);

                if (!Enum.TryParse<StaffStatus>(request.Status, true, out var staffStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

                if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidMaritalStatus);

                var doctor = new Doctor
                {
                    Address = request.Address,
                    FullName = request.FullName,
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email,
                    Gender = gender,
                    Phone = request.Phone,
                    NationalId = request.NationalId,
                    //DepartmentId = request.DepartmentId,
                    //SpecialtyId = request.SpecialtyId,
                    MaritalStatus = maritalStatus,
                    StartDate = request.StartDate,
                    Status = staffStatus,
                    Degree = request.Degree,
                    Notes = request.Notes,
                    MedicalServiceId = request.MedicalServiceId,
                    
                };

                if (request.Photo is not null)
                {
                    var photoUrl = await _fileService.UploadFileAsync(request.Photo, "doctors");

                    doctor.PhotoUrl = photoUrl;
                }


                await _unitOfWork.Repository<Doctor>().AddAsync(doctor, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                if (request.DoctorSchedules is not null && request.DoctorSchedules.Count > 0)
                {
                    var schedules = new List<DoctorSchedule>();

                    foreach (var schedule in request.DoctorSchedules)
                    {
                        var doctorSchedule = new DoctorSchedule
                        {
                            DoctorId = doctor.Id,
                            WeekDay = schedule.WeekDay,
                            StartTime = schedule.StartTime,
                            EndTime = schedule.EndTime,
                            Capacity = schedule.Capacity,
                        };

                        schedules.Add(doctorSchedule);
                    }

                    await _unitOfWork.Repository<DoctorSchedule>().AddRangeAsync(schedules, cancellationToken);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);

        }

        public Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        //public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var Params = new SqlParameter[3];
        //        Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
        //        Params[1] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
        //        Params[2] = new SqlParameter("@PageSize", pagingFilter.PageSize);
        //        var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllDoctors", Params);
        //        int totalCount = 0;
        //        if (dt.Rows.Count > 0)
        //        {
        //            int.TryParse(dt.Rows[0]["TotalCount"]?.ToString(), out totalCount);
        //        }

        //        return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);

        //    }
        //    catch (Exception)
        //    {
        //        return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        //    }
        //}

        public async Task<PagedResponseModel<List<AllDoctorsResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var Params = new SqlParameter[3];
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[2] = new SqlParameter("@PageSize", pagingFilter.PageSize);

                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllDoctors", Params);

                var doctors = dt.AsEnumerable().Select(row => new AllDoctorsResponse
                {
                    Id = row.Field<int>("DoctorId"), 
                    FullName = row.Field<string>("FullName") ?? string.Empty, 
                    Phone = row.Field<string>("Phone") ?? string.Empty,
                    Status = row.Field<string>("Status") ?? string.Empty,
                    DepartmentId = row.Field<int?>("DepartmentId") ?? 0, 
                    Department = row.Field<string>("Department") ?? string.Empty,
                    MedicalServiceId = row.Field<int?>("MedicalServiceId") ?? 0,
                    MedicalServiceName = row.Field<string>("MedicalServiceName") ?? string.Empty,
                    DoctorSchedules = JsonConvert.DeserializeObject<List<DoctorScheduleResponse>>(row.Field<string>("DoctorSchedules") ?? "[]") 
                }).ToList();

                int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int?>("TotalCount") ?? 0 : 0;

                return PagedResponseModel<List<AllDoctorsResponse>>.Success(GenericErrors.GetSuccess, totalCount, doctors);
            }
            catch (Exception)
            {
                return PagedResponseModel<List<AllDoctorsResponse>>.Failure(GenericErrors.TransFailed);
            }
        }


        public async Task<ErrorResponseModel<DoctorResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var doctor = await _unitOfWork.Repository<Doctor>()
                .GetAll(x => x.Id == id)
                .Include(x => x.Department)
                .Include(x => x.Specialty)
                .Include(x => x.Schedules)
                .Include(x => x.MedicalService)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(cancellationToken);

            if (doctor is null)
                return ErrorResponseModel<DoctorResponse>.Failure(GenericErrors.NotFound);

            var doctorResponse = new DoctorResponse
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                DateOfBirth = doctor.DateOfBirth,
                IsActive = doctor.IsActive,
                Address = doctor.Address,
                Department = doctor?.Department?.Name,
                DepartmentId = doctor?.DepartmentId,
                Email = doctor.Email,
                Gender = doctor.Gender.ToString(),
                NationalId = doctor.NationalId,
                Phone = doctor.Phone,
                PhotoUrl = doctor.PhotoUrl,
                Specialty = doctor?.Specialty?.Name,
                SpecialtyId = doctor.SpecialtyId,
                StartDate = doctor.StartDate,
                Degree = doctor.Degree,
                Notes = doctor.Notes,
                Status = doctor.Status.GetArabicValue(),
                MaritalStatus = doctor.MaritalStatus.GetArabicValue(),
                MedicalServiceId = doctor.MedicalServiceId,
                MedicalServiceName = doctor?.MedicalService?.Name,

                DoctorSchedules = [.. doctor.Schedules.Select(schedule => new DoctorScheduleResponse
                {
                    WeekDay = schedule.WeekDay,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    Id = schedule.Id,
                    Capacity = schedule.Capacity
                })],

                CreatedAt = doctor.CreatedOn,
                CreatedBy = $"{doctor.CreatedBy?.FirstName} {doctor.CreatedBy?.LastName}",
                UpdatedAt = doctor.UpdatedOn,
                UpdatedBy = doctor.UpdatedBy != null ?
                    $"{doctor.UpdatedBy.FirstName} {doctor.UpdatedBy.LastName}" :
                    string.Empty
            };

            return ErrorResponseModel<DoctorResponse>.Success(GenericErrors.GetSuccess, doctorResponse);

        }

        public async Task<PagedResponseModel<DataTable>> GetCountsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetDoctorTypeCountStatistics", Array.Empty<SqlParameter>());
                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, 3, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }


        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, DoctorRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var doctor = await _unitOfWork.Repository<Doctor>().GetAll(i => i.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (doctor is null)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidGender);

                if (!Enum.TryParse<StaffStatus>(request.Status, true, out var staffStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidStatus);

                if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
                    return ErrorResponseModel<string>.Failure(GenericErrors.InvalidMaritalStatus);

                doctor.Address = request.Address;
                doctor.FullName = request.FullName;
                doctor.DateOfBirth = request.DateOfBirth;
                doctor.Email = request.Email;
                doctor.Gender = gender;
                doctor.Phone = request.Phone;
                doctor.NationalId = request.NationalId;
                //doctor.DepartmentId = request.DepartmentId;
                //doctor.SpecialtyId = request.SpecialtyId;
                doctor.MaritalStatus = maritalStatus;
                doctor.StartDate = request.StartDate;
                doctor.Status = staffStatus;
                doctor.Degree = request.Degree;
                doctor.Notes = request.Notes;
                doctor.MedicalServiceId = request.MedicalServiceId;

                if (request.Photo != null && request.Photo.Length > 0)
                {
                    if (!string.IsNullOrEmpty(doctor.PhotoUrl))
                    {
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "doctors", doctor.PhotoUrl);
                        if (File.Exists(imagePath))
                            File.Delete(imagePath);
                    }

                    var newPhoto = await _fileService.UploadFileAsync(request.Photo, "doctors");
                    doctor.PhotoUrl = newPhoto;
                }

                _unitOfWork.Repository<Doctor>().Update(doctor);
                await _unitOfWork.CompleteAsync(cancellationToken);

                var existingSchedules = await _unitOfWork.Repository<DoctorSchedule>()
                    .GetAll()
                    .Where(ds => ds.DoctorId == doctor.Id)
                    .ToListAsync(cancellationToken);

                _unitOfWork.Repository<DoctorSchedule>().DeleteRange(existingSchedules);
                await _unitOfWork.CompleteAsync(cancellationToken);

                if (request.DoctorSchedules is not null && request.DoctorSchedules.Count > 0)
                {
                    var newSchedules = request.DoctorSchedules
                        //.Where(schedule => schedule.IsWorking) 
                        .Select(schedule => new DoctorSchedule
                        {
                            DoctorId = doctor.Id,
                            WeekDay = schedule.WeekDay,
                            StartTime = schedule.StartTime,
                            EndTime = schedule.EndTime
                        })
                        .ToList();

                    await _unitOfWork.Repository<DoctorSchedule>().AddRangeAsync(newSchedules, cancellationToken);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
        }

        public async Task ResetCurrentAppointmentsAsync()
        {
            var schedules = await _unitOfWork.Repository<DoctorSchedule>()
                .GetAll()
                .ToListAsync();

            foreach (var schedule in schedules)
            {
                schedule.CurrentAppointments = 0;
                _unitOfWork.Repository<DoctorSchedule>().Update(schedule);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
