using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Core.Common;
using Hospital_MS.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Services.Common;

namespace Hospital_MS.Services.HMS
{
    public class PatientAttachmentService(IUnitOfWork unitOfWork, IFileService fileService, IWebHostEnvironment webHostEnvironment) : IPatientAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<ErrorResponseModel<string>> CreateAsync(int patientId, PatientAttachmentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var patientIsExist = await _unitOfWork.Repository<Patient>().AnyAsync(x => x.Id == patientId, cancellationToken);

                if (!patientIsExist)
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                var fileUrl = await _fileService.UploadFileAsync(request.File, "patients");

                var attachment = new PatientAttachment
                {
                    PatientId = patientId,
                    AttachmentUrl = fileUrl,
                };

                await _unitOfWork.Repository<PatientAttachment>().AddAsync(attachment, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var attachment = await _unitOfWork.Repository<PatientAttachment>().GetAll(i => i.Id == id).FirstOrDefaultAsync();

                if (attachment is not { })
                    return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

                if (attachment.AttachmentUrl.Length > 0)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "patients", attachment.AttachmentUrl);

                    filePath = $"wwwroot{filePath}";

                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }

                _unitOfWork.Repository<PatientAttachment>().Delete(attachment);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<List<PatientAttachmentResponse>>> GetAllAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var attachments = await _unitOfWork.Repository<PatientAttachment>().GetAll(i => i.PatientId == patientId).Include(x => x.Patient).Include(x => x.CreatedBy).Include(x => x.UpdatedBy).ToListAsync();

            var response = attachments.Select(attachment => new PatientAttachmentResponse
            {
                Id = attachment.Id,
                AttachmentUrl = attachment.AttachmentUrl,
                CreatedOn = attachment.CreatedOn,
                CreatedBy = $"{attachment.CreatedBy?.FirstName} {attachment.CreatedBy?.LastName}",
                UpdatedOn = attachment.UpdatedOn,
                UpdatedBy = attachment.UpdatedBy != null ? $"{attachment.UpdatedBy.FirstName} {attachment.UpdatedBy.LastName}" : string.Empty

            }).ToList();

            return ErrorResponseModel<List<PatientAttachmentResponse>>.Success(GenericErrors.GetSuccess, response);
        }
    }
}
