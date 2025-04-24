using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Doctors;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Services.Common;
using Hospital_MS.Core.Specifications.Doctors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class DoctorService(IUnitOfWork unitOfWork, IFileService fileService, IWebHostEnvironment webHostEnvironment) : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<Result> CreateAsync(DoctorRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return Result.Failure(new Error("InvalidGender", "Invalid Gender provided.", 400));

                if (!Enum.TryParse<StaffStatus>(request.Status, true, out var staffStatus))
                    return Result.Failure(new Error("InvalidStatus", "Invalid staff Status provided.", 400));

                if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
                    return Result.Failure(new Error("InvalidMaritalStatus", "Invalid MaritalStatus provided.", 400));

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

                // Save Doctor Schedules

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
                return Result.Failure(GenericErrors<Doctor>.FailedToAdd);
            }

            return Result.Success();

        }

        public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IReadOnlyList<AllDoctorsResponse>>> GetAllAsync(GetDoctorsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new DoctorSpecification(request);

            var doctors = await _unitOfWork.Repository<Doctor>().GetAllWithSpecAsync(spec, cancellationToken);

            var response = doctors.Select(doc => new AllDoctorsResponse
            {
                Id = doc.Id,
                FullName = doc.FullName,
                Phone = doc.Phone,
                Department = doc.Department.Name,
                Status = doc.Status.ToString()
            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<AllDoctorsResponse>>(response);
        }

        public Task<int> GetAllCountAsync(GetDoctorsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new DoctorsCountSpecification(request);

            return _unitOfWork.Repository<Doctor>().GetCountAsync(spec, cancellationToken);
        }

        public async Task<Result<DoctorResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var spec = new DoctorSpecification(id);

            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdWithSpecAsync(spec, cancellationToken);

            if (doctor is null)
                return Result.Failure<DoctorResponse>(GenericErrors<Doctor>.NotFound);

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

            return Result.Success(doctorResponse);

        }

        public async Task<Result<DoctorsCountResponse>> GetCountsAsync(CancellationToken cancellationToken = default)
        {
            var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsync(cancellationToken);
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync(cancellationToken);

            var response = new DoctorsCountResponse
            {
                TotalActiveDoctors = doctors.Count(d => d.Status == StaffStatus.Active),
                TotalDepartments = departments.Count,
                TotalDoctors = doctors.Count,
            };

            return Result.Success(response);
        }


        public async Task<Result> UpdateAsync(int id, DoctorRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id, cancellationToken);

                if (doctor is null)
                    return Result.Failure(new Error("NotFound", "Doctor not found.", 404));

                if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
                    return Result.Failure(new Error("InvalidGender", "Invalid Gender provided.", 400));

                if (!Enum.TryParse<StaffStatus>(request.Status, true, out var staffStatus))
                    return Result.Failure(new Error("InvalidStatus", "Invalid staff Status provided.", 400));

                if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
                    return Result.Failure(new Error("InvalidMaritalStatus", "Invalid MaritalStatus provided.", 400));

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

                // Update Doctor Schedules based on the provided schedule data

                var existingSchedules = await _unitOfWork.Repository<DoctorSchedule>()
                    .GetAllAsQueryable()
                    .Where(ds => ds.DoctorId == doctor.Id)
                    .ToListAsync(cancellationToken);

                _unitOfWork.Repository<DoctorSchedule>().DeleteRange(existingSchedules);
                await _unitOfWork.CompleteAsync(cancellationToken);

                if (request.DoctorSchedules is not null && request.DoctorSchedules.Count > 0)
                {
                    var newSchedules = request.DoctorSchedules
                        .Where(schedule => schedule.IsWorking) 
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
                return Result.Failure(GenericErrors<Doctor>.FailedToUpdate);
                //return Result.Failure(new Error("Failed", ex.Message, 400));
            }

            return Result.Success();
        }
    }
}
