using Hospital_MS.Core.Contracts.Emergency;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Models.Medical;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

public class EmergencyService : IEmergencyService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmergencyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EmergencyVisitResponseDto> CreateEmergencyVisitAsync(
    EmergencyVisitCreateDto dto, CancellationToken cancellationToken)
    {
        var patientRepo = _unitOfWork.Repository<Patient>();
        var visitRepo = _unitOfWork.Repository<EmergencyVisit>();
        var historyRepo = _unitOfWork.Repository<PatientMedicalHistory>();

        Patient patient;

        var existingPatient = await patientRepo
            .GetAll(x => x.Phone == dto.Phone)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingPatient != null)
        {
            existingPatient.LastVisitDate = DateTime.UtcNow;
            existingPatient.Status = PatientStatus.Emergency;

            patient = existingPatient;
        }
        else
        {
            patient = new Patient
            {
                FullName = dto.PatientName,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Status = PatientStatus.Emergency,
                LastVisitDate = DateTime.UtcNow
            };

            await patientRepo.AddAsync(patient, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        var historyEntry = new PatientMedicalHistory
        {
            PatientId = patient.Id,
            Description = $"تم تسجيل زيارة طوارئ للمريض، والشكوى الرئيسية: {dto.ChiefComplaint}",
            CreatedOn = DateTime.UtcNow
        };

        await historyRepo.AddAsync(historyEntry, cancellationToken);

        var visit = new EmergencyVisit
        {
            PatientId = patient.Id,
            ChiefComplaint = dto.ChiefComplaint,
            CompanionName = dto.CompanionName,
            CompanionPhone = dto.CompanionPhone,
            CompanionNationalId = dto.CompanionNationalId,
            Status = EmergencyStatus.Waiting
        };

        await visitRepo.AddAsync(visit, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponse(visit);
    }

    public async Task<EmergencyVisitResponseDto> AddTriageAsync(
        EmergencyTriageDto dto, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<EmergencyVisit>();
        var visit = await repo.GetAll(x => x.Id == dto.EmergencyVisitId)
                              .FirstOrDefaultAsync(cancellationToken);
        var historyRepo = _unitOfWork.Repository<PatientMedicalHistory>();

        if (visit == null)
            throw new Exception("Emergency visit not found.");

        visit.Severity = dto.Severity;
        visit.BloodPressure = dto.BloodPressure;
        visit.HeartRate = dto.HeartRate;
        visit.RespiratoryRate = dto.RespiratoryRate;
        visit.Temperature = dto.Temperature;
        visit.OxygenSaturation = dto.OxygenSaturation;
        visit.PainScore = dto.PainScore;
        visit.Allergies = dto.Allergies;

        visit.Status = EmergencyStatus.UnderTriage;

        if (dto.Severity == EmergencySeverity.Level1_Resuscitation)
            visit.Status = EmergencyStatus.UnderAssessment;

        await historyRepo.AddAsync(new PatientMedicalHistory
        {
            PatientId = visit.PatientId,
            Description = $"تم عمل تقييم – مستوى الخطورة: {dto.Severity}",
            CreatedOn = DateTime.UtcNow
        }, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponse(visit);
    }

    public async Task<EmergencyVisitResponseDto> AssignDoctorAsync(
        EmergencyAssignDoctorDto dto, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<EmergencyVisit>();
        var visit = await repo.GetAll(x => x.Id == dto.EmergencyVisitId)
                              .FirstOrDefaultAsync(cancellationToken);
        var historyRepo = _unitOfWork.Repository<PatientMedicalHistory>();

        if (visit == null)
            throw new Exception("Emergency visit not found.");

        visit.DoctorId = dto.DoctorId;
        if (visit.Status == EmergencyStatus.Waiting ||
            visit.Status == EmergencyStatus.UnderTriage)
        {
            visit.Status = EmergencyStatus.UnderAssessment;
        }
        await historyRepo.AddAsync(new PatientMedicalHistory
        {
            PatientId = visit.PatientId,
            Description = $"تم إسناد الحالة للطبيب رقم {dto.DoctorId}",
            CreatedOn = DateTime.UtcNow
        }, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponse(visit);
    }

    public async Task<EmergencyVisitResponseDto> UpdateStatusAsync(
        int id, EmergencyStatus newStatus, CancellationToken cancellationToken)
    {
        var visitRepo = _unitOfWork.Repository<EmergencyVisit>();
        var patientRepo = _unitOfWork.Repository<Patient>();
        var historyRepo = _unitOfWork.Repository<PatientMedicalHistory>();
        var admissionRepo = _unitOfWork.Repository<Admission>();

        var visit = await visitRepo
            .GetAll(x => x.Id == id)
            .Include(x => x.Patient)
            .FirstOrDefaultAsync(cancellationToken);

        if (visit == null)
            throw new Exception("Emergency visit not found.");

        if (newStatus == EmergencyStatus.UnderAssessment && visit.DoctorId == null)
            throw new Exception("Cannot move to UnderAssessment without assigning a doctor.");

        visit.Status = newStatus;

        switch (newStatus)
        {
            case EmergencyStatus.Waiting:
            case EmergencyStatus.UnderTriage:
            case EmergencyStatus.UnderAssessment:
            case EmergencyStatus.UnderTreatment:
                visit.Patient.Status = PatientStatus.Emergency;
                break;

            case EmergencyStatus.ReadyForDischarge:
                visit.Patient.Status = PatientStatus.Emergency;
                break;

            case EmergencyStatus.Discharged:
                visit.Patient.Status = PatientStatus.Active;
                visit.IsAdmitted = false;
                visit.DischargeTime = DateTime.UtcNow;
                break;

            case EmergencyStatus.Admitted:
                visit.Patient.Status = PatientStatus.Inpatient;
                visit.IsAdmitted = true;

                var admission = new Admission
                {
                    PatientId = visit.PatientId,
                    DoctorId = visit.DoctorId ?? 0,
                    InitialDiagnosis = visit.ChiefComplaint,
                    AdmissionDate = DateTime.UtcNow,
                    Status = AdmissionStatus.Admitted,
                    EncounterNumber = visit.EncounterNumber,
                    Notes = "حجز داخلي مُحول من الطوارئ",
                };

                await admissionRepo.AddAsync(admission, cancellationToken);
                visit.AdmissionId = admission.Id;
                break;

            case EmergencyStatus.Transferred:
                visit.Patient.Status = PatientStatus.Inpatient;
                break;
        }

        await historyRepo.AddAsync(new PatientMedicalHistory
        {
            PatientId = visit.PatientId,
            Description = $"تم تغيير حالة الطوارئ إلى: {newStatus}",
            CreatedOn = DateTime.UtcNow
        }, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return MapToResponse(visit);
    }

    public async Task<EmergencyVisitResponseDto?> GetByIdAsync(
        int id, CancellationToken cancellationToken)
    {
        var visit = await _unitOfWork.Repository<EmergencyVisit>()
            .GetAll(x => x.Id == id)
            .Include(x => x.Patient)
            .Include(x => x.Doctor)
            .FirstOrDefaultAsync(cancellationToken);

        return visit == null ? null : MapToResponse(visit);
    }

    public async Task<List<EmergencyVisitResponseDto>> GetActiveVisitsAsync(CancellationToken cancellationToken)
    {
        var inactiveStatuses = new[]
        {
        EmergencyStatus.Discharged,
        EmergencyStatus.Admitted,
        EmergencyStatus.Transferred
    };

        var visits = await _unitOfWork.Repository<EmergencyVisit>()
            .GetAll(x => !inactiveStatuses.Contains(x.Status))
            .Include(x => x.Patient)
            .Include(x => x.Doctor)
            .ToListAsync(cancellationToken);

        return visits.Select(MapToResponse).ToList();
    }

    private EmergencyVisitResponseDto MapToResponse(EmergencyVisit visit)
    {
        return new EmergencyVisitResponseDto
        {
            Id = visit.Id,
            EncounterNumber = visit.EncounterNumber,
            PatientId = visit.PatientId,
            PatientName = visit.Patient?.FullName ?? "",
            ArrivalTime = visit.ArrivalTime,
            ChiefComplaint = visit.ChiefComplaint,
            Severity = visit.Severity,
            IsAdmitted = visit.IsAdmitted,
            AdmissionId = visit.AdmissionId,

            BloodPressure = visit.BloodPressure,
            HeartRate = visit.HeartRate,
            RespiratoryRate = visit.RespiratoryRate,
            Temperature = visit.Temperature,
            OxygenSaturation = visit.OxygenSaturation,
            PainScore = visit.PainScore,
            Allergies = visit.Allergies,
            DoctorName = visit.Doctor?.FullName
        };
    }
}
