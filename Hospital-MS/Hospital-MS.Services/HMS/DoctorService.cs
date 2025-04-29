using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Doctors;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Services;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class DoctorService(IUnitOfWork unitOfWork, IFileService fileService, IWebHostEnvironment webHostEnvironment) : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

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
                    DepartmentId = request.DepartmentId,
                    SpecialtyId = request.SpecialtyId,
                    MaritalStatus = maritalStatus,
                    StartDate = request.StartDate,
                    Status = staffStatus,
                    Degree = request.Degree,
                    Notes = request.Notes,
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

        public async Task<ErrorResponseModel<List<AllDoctorsResponse>>> GetAllAsync(GetDoctorsRequest request, CancellationToken cancellationToken = default)
        {
            var doctors = await _unitOfWork.Repository<Doctor>().GetAll().ToListAsync(cancellationToken: cancellationToken);

            var response = doctors.Select(doc => new AllDoctorsResponse
            {
                Id = doc.Id,
                FullName = doc.FullName,
                Phone = doc.Phone,
                Department = doc?.Department?.Name,
                Status = doc.Status.ToString()

            }).ToList();

            return ErrorResponseModel<List<AllDoctorsResponse>>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<ErrorResponseModel<DoctorResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetAll().FirstOrDefaultAsync();

            if (doctor is null)
                return ErrorResponseModel<DoctorResponse>.Failure(GenericErrors.NotFound);

            var doctorResponse = new DoctorResponse
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                DateOfBirth = doctor.DateOfBirth,
                IsActive = doctor.IsActive,
                Address = doctor.Address,
                Department = doctor.Department.Name,
                DepartmentId = doctor.DepartmentId,
                Email = doctor.Email,
                Gender = doctor.Gender.ToString(),
                NationalId = doctor.NationalId,
                Phone = doctor.Phone,
                PhotoUrl = doctor.PhotoUrl,
                Specialty = doctor.Specialty.Name,
                SpecialtyId = doctor.SpecialtyId,
                StartDate = doctor.StartDate,
                Degree = doctor.Degree,
                Notes = doctor.Notes,
                Status = doctor.Status.ToString(),
                MaritalStatus = doctor.MaritalStatus.ToString(),

                DoctorSchedules = [.. doctor.Schedules.Select(schedule => new DoctorScheduleResponse
                {
                    WeekDay = schedule.WeekDay,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime
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

        public async Task<ErrorResponseModel<DoctorsCountResponse>> GetCountsAsync(CancellationToken cancellationToken = default)
        {
            var doctors = await _unitOfWork.Repository<Doctor>().GetAll().ToListAsync();
            var departments = await _unitOfWork.Repository<Department>().GetAll().ToListAsync();

            var response = new DoctorsCountResponse
            {
                TotalActiveDoctors = doctors.Count(d => d.Status == StaffStatus.Active),
                TotalDepartments = departments.Count,
                TotalDoctors = doctors.Count,
            };

            return ErrorResponseModel<DoctorsCountResponse>.Success(GenericErrors.GetSuccess, response);
        }


        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, DoctorRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var doctor = await _unitOfWork.Repository<Doctor>().GetAll(i => i.Id == id).FirstOrDefaultAsync();

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
                doctor.DepartmentId = request.DepartmentId;
                doctor.SpecialtyId = request.SpecialtyId;
                doctor.MaritalStatus = maritalStatus;
                doctor.StartDate = request.StartDate;
                doctor.Status = staffStatus;
                doctor.Degree = request.Degree;
                doctor.Notes = request.Notes;

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
    }
}
