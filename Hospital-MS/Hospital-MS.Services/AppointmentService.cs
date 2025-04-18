using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Helpers;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.Appointments;

namespace Hospital_MS.Services
{
    public class AppointmentService(IUnitOfWork unitOfWork) : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> CreateAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (!Enum.TryParse<AppointmentType>(request.AppointmentType, true, out var appointmentType))
                    return Result.Failure(new Error("InvalidType", "Invalid appointment type provided.", 400));


                var patient = new Patient
                {
                    FullName = ArabicNormalizer.NormalizeArabic(request.PatientName),
                    Phone = request.PatientPhone,
                    InsuranceCompanyId = request.InsuranceCompanyId,
                    InsuranceCategoryId = request.InsuranceCategoryId,
                    Status = PatientStatus.Outpatient,
                };

                await _unitOfWork.Repository<Patient>().AddAsync(patient, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                var appointment = new Appointment
                {
                    PatientId = patient.Id,
                    DoctorId = request.DoctorId,
                    AppointmentDateTime = request.AppointmentDate,
                    PaymentMethod = request.PaymentMethod,
                    Type = appointmentType,
                    ClinicId = request.ClinicId,

                    EmergencyLevel = request.EmergencyLevel,
                    CompanionName = request.CompanionName,
                    CompanionNationalId = request.CompanionNationalId,
                    CompanionPhone = request.CompanionPhone
                };

                await _unitOfWork.Repository<Appointment>().AddAsync(appointment, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(GenericErrors<Appointment>.FailedToAdd);
            }
        }

        public async Task<Result<IReadOnlyList<AppointmentResponse>>> GetAllAsync(GetAppointmentsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new AppointmentSpecification(request);

            var appointments = await _unitOfWork.Repository<Appointment>().GetAllWithSpecAsync(spec, cancellationToken);

            var response = appointments.Select(app => new AppointmentResponse
            {
                Id = app.Id,
                PatientName = app.Patient.FullName,
                DoctorName = app?.Doctor?.FullName,
                AppointmentDate = app.AppointmentDateTime,
                PaymentMethod = app.PaymentMethod,
                Status = app.Status.ToString(),
                Type = app.Type.ToString(),
                DoctorId = app.DoctorId,
                PatientId = app.PatientId,
                CreatedOn = app.CreatedOn,
                UpdatedOn = app.UpdatedOn,
                CreatedBy = $"{app.CreatedBy.FirstName} {app.CreatedBy.LastName}",
                UpdatedBy = $"{app?.UpdatedBy?.FirstName} {app?.UpdatedBy?.LastName}" ?? string.Empty,
                PatientPhone = app?.Patient?.Phone,
                ClinicId = app.ClinicId,
                ClinicName = app?.Clinic?.Name,
            }).ToList().AsReadOnly();

            return Result.Success<IReadOnlyList<AppointmentResponse>>(response);
        }

        public async Task<int> GetAppointmentsCountAsync(GetAppointmentsRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new AppointmentsCountSpecification(request);

            return await _unitOfWork.Repository<Appointment>().GetCountAsync(spec, cancellationToken);
        }

        public async Task<Result<AppointmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var spec = new AppointmentSpecification(id);

            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdWithSpecAsync(spec, cancellationToken);

            if (appointment == null)
            {
                return Result.Failure<AppointmentResponse>(GenericErrors<Appointment>.NotFound);
            }

            var response = new AppointmentResponse
            {
                Id = appointment.Id,
                PatientName = appointment.Patient.FullName,
                DoctorName = appointment?.Doctor?.FullName,
                AppointmentDate = appointment.AppointmentDateTime,
                PaymentMethod = appointment.PaymentMethod,
                Status = appointment.Status.ToString(),
                Type = appointment.Type.ToString(),
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                CreatedOn = appointment.CreatedOn,
                UpdatedOn = appointment.UpdatedOn,
                CreatedBy = $"{appointment.CreatedBy.FirstName} {appointment.CreatedBy.LastName}",
                UpdatedBy = $"{appointment?.UpdatedBy?.FirstName} {appointment?.UpdatedBy?.LastName}" ?? string.Empty,
                PatientPhone = appointment?.Patient?.Phone,

                CompanionName = appointment.CompanionName,
                CompanionNationalId = appointment.CompanionNationalId,
                CompanionPhone = appointment.CompanionPhone,
                EmergencyLevel = appointment.EmergencyLevel,
                ClinicId = appointment.ClinicId,
                ClinicName = appointment?.Clinic?.Name,

            };

            return Result.Success(response);
        }

        public async Task<Result<AppointmentCountsResponse>> GetCountsAsync(CancellationToken cancellationToken = default)
        {
            var appointments = await _unitOfWork.Repository<Appointment>().GetAllAsync(cancellationToken);

            var response = new AppointmentCountsResponse
            {
                EmergencyCount = appointments.Count(a => a.Type == AppointmentType.Emergency),
                ScreeningCount = appointments.Count(a => a.Type == AppointmentType.Screening),
                RadiologyCount = appointments.Count(a => a.Type == AppointmentType.Radiology),
                SurgeryCount = appointments.Count(a => a.Type == AppointmentType.Surgery),
                ConsultationCount = appointments.Count(a => a.Type == AppointmentType.Consultation),
                GeneralCount = appointments.Count(a => a.Type == AppointmentType.General)
            };

            return Result.Success(response);
        }

        public async Task<Result> UpdateAsync(int id, UpdateAppointmentRequest request, CancellationToken cancellationToken = default)
        {

            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id, cancellationToken);

            if (appointment == null)
                return Result.Failure(GenericErrors<Appointment>.NotFound);


            if (!Enum.TryParse<AppointmentType>(request.AppointmentType, true, out var appointmentType))
                return Result.Failure(new Error("InvalidType", "Invalid appointment type provided.", 400));

            appointment.DoctorId = request.DoctorId;
            //appointment.ClinicId = request.ClinicId;
            appointment.AppointmentDateTime = request.AppointmentDate;
            appointment.PaymentMethod = request.PaymentMethod;
            appointment.Type = appointmentType;

            _unitOfWork.Repository<Appointment>().Update(appointment);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateStatusAsync(int id, UpdatePatientStatusInEmergencyRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new AppointmentSpecification(id);

            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdWithSpecAsync(spec, cancellationToken);

            if (appointment is not { })
                return Result.Failure(GenericErrors<Appointment>.NotFound);

            if (appointment.Type != AppointmentType.Emergency)
                return Result.Failure(new Error("Appointment.NotEmergency", "This action only allowed to Emergency Type", 400));

            if (request.NewStatus == "General")
            {
                var newType = Enum.Parse<AppointmentType>(request.NewStatus);
                appointment.Type = newType;
                request.NewStatus = "Outpatient";
            }

            if (!Enum.TryParse<PatientStatus>(request.NewStatus, true, out var newStatus))
                return Result.Failure(new Error("Patient.InvalidStatus", $"Invalid Status: {request.NewStatus}", 400));

            appointment.Patient.Status = newStatus;

            _unitOfWork.Repository<Appointment>().Update(appointment);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
