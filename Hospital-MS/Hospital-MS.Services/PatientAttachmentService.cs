using Hospital_MS.Core.Abstractions;
using Hospital_MS.Core.Contracts.Patients;
using Hospital_MS.Core.Errors;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Repositories;
using Hospital_MS.Core.Services.Common;
using Hospital_MS.Core.Services;
using Hospital_MS.Core.Specifications.PatientAttachments;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services
{
    public class PatientAttachmentService(IUnitOfWork unitOfWork, IFileService fileService, IWebHostEnvironment webHostEnvironment) : IPatientAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<Result> CreateAsync(int patientId, PatientAttachmentRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var patientIsExist = await _unitOfWork.Repository<Patient>().AnyAsync(x => x.Id == patientId, cancellationToken);

                if (!patientIsExist)
                    return Result.Failure(GenericErrors<Patient>.NotFound);

                var fileUrl = await _fileService.UploadFileAsync(request.File, "patients");

                var attachment = new PatientAttachment
                {
                    PatientId = patientId,
                    AttachmentUrl = fileUrl,
                };

                await _unitOfWork.Repository<PatientAttachment>().AddAsync(attachment, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<PatientAttachment>.FailedToAdd);
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var attachment = await _unitOfWork.Repository<PatientAttachment>().GetByIdAsync(id, cancellationToken);

                if (attachment is not { })
                    return Result.Failure(GenericErrors<PatientAttachment>.NotFound);

                if (attachment.AttachmentUrl.Length > 0)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "patients", attachment.AttachmentUrl);

                    filePath = $"wwwroot{filePath}";

                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }

                _unitOfWork.Repository<PatientAttachment>().Delete(attachment);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(GenericErrors<PatientAttachment>.FailedToDelete);
            }
        }

        public async Task<Result<IReadOnlyList<PatientAttachmentResponse>>> GetAllAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var spec = new PatientAttachmentSpecification(patientId);

            var attachments = await _unitOfWork.Repository<PatientAttachment>().GetAllWithSpecAsync(spec, cancellationToken);

            var response = attachments.Select(attachment => new PatientAttachmentResponse
            {
                Id = attachment.Id,
                AttachmentUrl = attachment.AttachmentUrl,
                CreatedOn = attachment.CreatedOn,
                CreatedBy = $"{attachment.CreatedBy?.FirstName} {attachment.CreatedBy?.LastName}",
                UpdatedOn = attachment.UpdatedOn,
                UpdatedBy = attachment.UpdatedBy != null ?
                    $"{attachment.UpdatedBy.FirstName} {attachment.UpdatedBy.LastName}" :
                    string.Empty

            }).ToList();

            return Result.Success<IReadOnlyList<PatientAttachmentResponse>>(response);
        }
    }
}
