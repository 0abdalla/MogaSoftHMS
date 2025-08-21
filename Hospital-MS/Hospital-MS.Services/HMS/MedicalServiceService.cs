using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MedicalServices;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace Hospital_MS.Services.HMS
{
    public class MedicalServiceService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IMedicalServiceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateMedicalService(MedicalServiceRequest request, CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var isExist = await _unitOfWork.Repository<MedicalService>().AnyAsync(x => x.Name == request.Name && x.Type == request.Type, cancellationToken);

                if (isExist)
                    return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                var medicalService = new MedicalService
                {
                    Name = request.Name,
                    Price = request.Price,
                    Type = request.Type,
                };

                await _unitOfWork.Repository<MedicalService>().AddAsync(medicalService, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                if (request.Type == "Radiology" && !string.IsNullOrEmpty(request.RadiologyBodyTypeName))
                {
                    var bodyType = new RadiologyBodyType
                    {
                        MedicalServiceId = medicalService.Id,
                        Name = request.Name,
                    };

                    await _unitOfWork.Repository<RadiologyBodyType>().AddAsync(bodyType, cancellationToken);
                }

                if (request.WeekDays is not null && request.WeekDays.Count > 0)
                {
                    var schedules = new List<MedicalServiceSchedule>();

                    foreach (var WeekDay in request.WeekDays)
                    {
                        var medicalSchedule = new MedicalServiceSchedule
                        {
                            MedicalServiceId = medicalService.Id,
                            WeekDay = WeekDay,
                        };

                        schedules.Add(medicalSchedule);
                    }

                    await _unitOfWork.Repository<MedicalServiceSchedule>().AddRangeAsync(schedules, cancellationToken);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, medicalService.Id.ToString());

            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);

                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<ErrorResponseModel<string>> CreateRadiologyBodyType(RadiologyBodyTypeRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var isExist = await _unitOfWork.Repository<RadiologyBodyType>().AnyAsync(x => x.Name == request.Name && x.MedicalServiceId == request.MedicalServiceId, cancellationToken);

                if (isExist)
                    return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

                var medicalService = new RadiologyBodyType
                {
                    Name = request.Name,
                    MedicalServiceId = request.MedicalServiceId,
                };

                await _unitOfWork.Repository<RadiologyBodyType>().AddAsync(medicalService, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);


                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, medicalService.Id.ToString());

            }
            catch (Exception)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }
        }

        public async Task<PagedResponseModel<List<MedicalServiceResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                var SearchText = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "SearchText")?.ItemValue;
                var Params = new SqlParameter[3];
                Params[0] = new SqlParameter("@SearchText", SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[2] = new SqlParameter("@PageSize", pagingFilter.PageSize);

                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetMedicalServices", Params);

                var doctors = dt.AsEnumerable().Select(row => new MedicalServiceResponse
                {
                    Id = row.Field<int>("ServiceId"),
                    Name = row.Field<string>("ServiceName") ?? string.Empty,
                    Price = row.Field<decimal?>("Price") ?? 0,
                    Type = row.Field<string>("ServiceType") ?? string.Empty,
                    MedicalServiceSchedules = JsonConvert.DeserializeObject<List<MedicalServiceScheduleResponse>>(row.Field<string>("MedicalServiceSchedules") ?? "[]"),
                    RadiologyBodyTypes = JsonConvert.DeserializeObject<List<RadiologyBodyTypeResponse>>(row.Field<string>("RadiologyBodyTypes") ?? "[]")
                }).ToList();

                int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int?>("TotalCount") ?? 0 : 0;

                return PagedResponseModel<List<MedicalServiceResponse>>.Success(GenericErrors.GetSuccess, totalCount, doctors);
            }
            catch (Exception)
            {
                return PagedResponseModel<List<MedicalServiceResponse>>.Failure(GenericErrors.TransFailed);
            }
        }

        public Task<ErrorResponseModel<MedicalServiceResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, MedicalServiceRequest request, CancellationToken cancellationToken = default)
        {
            var medicalService = await _unitOfWork.Repository<MedicalService>().GetByIdAsync(id, cancellationToken);

            if (medicalService == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var isExist = await _unitOfWork.Repository<MedicalService>().AnyAsync(x => x.Name == request.Name && x.Type == request.Type && x.Id != id, cancellationToken);

            if (isExist)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            medicalService.Name = request.Name;
            medicalService.Price = request.Price;
            medicalService.Type = request.Type;

            _unitOfWork.Repository<MedicalService>().Update(medicalService);

            // Handle MedicalServiceSchedules

            var existingSchedules = _unitOfWork.Repository<MedicalServiceSchedule>().GetAll(x => x.MedicalServiceId == id).ToList();

            if (existingSchedules.Count != 0)
            {
                _unitOfWork.Repository<MedicalServiceSchedule>().DeleteRange(existingSchedules);
            }

            if (request.WeekDays is not null && request.WeekDays.Count > 0)
            {
                var newSchedules = request.WeekDays.Select(WeekDay => new MedicalServiceSchedule
                {
                    MedicalServiceId = id,
                    WeekDay = WeekDay

                }).ToList();

                await _unitOfWork.Repository<MedicalServiceSchedule>().AddRangeAsync(newSchedules, cancellationToken);
            }

            var result = await _unitOfWork.CompleteAsync(cancellationToken);

            if (result > 0)
                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, medicalService.Id.ToString());

            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }


        public async Task<ErrorResponseModel<List<RadiologyBodyType>>> GetRadiologyBodyTypes()
        {
            try
            {
                var response = await _unitOfWork.Repository<RadiologyBodyType>().GetAll().ToListAsync();

                return ErrorResponseModel<List<RadiologyBodyType>>.Success(GenericErrors.GetSuccess, response);
            }
            catch (Exception)
            {
                return ErrorResponseModel<List<RadiologyBodyType>>.Failure(GenericErrors.TransFailed);
            }
        }
    }
}
