using Hospital_MS.Core.Contracts.Emergency;
using Hospital_MS.Core.Enums;

namespace Hospital_MS.Interfaces.HMS;

public interface IEmergencyService
{
    Task<EmergencyVisitResponseDto> CreateEmergencyVisitAsync(EmergencyVisitCreateDto dto, CancellationToken cancellationToken);
    Task<EmergencyVisitResponseDto> AddTriageAsync(EmergencyTriageDto dto, CancellationToken cancellationToken);
    Task<EmergencyVisitResponseDto> AssignDoctorAsync(EmergencyAssignDoctorDto dto, CancellationToken cancellationToken);
    Task<EmergencyVisitResponseDto> UpdateStatusAsync(int id, EmergencyStatus newStatus, CancellationToken cancellationToken);
    Task<List<EmergencyVisitResponseDto>> GetActiveVisitsAsync(CancellationToken cancellationToken);
    Task<EmergencyVisitResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
