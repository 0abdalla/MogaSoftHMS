﻿using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MedicalServices;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class MedicalServiceController(IMedicalServiceService medicalServiceService) : ApiBaseController
    {
        private readonly IMedicalServiceService _medicalServiceService = medicalServiceService;

        [HttpPost("")]
        public async Task<IActionResult> CreateMedicalService([FromBody] MedicalServiceRequest request, CancellationToken cancellationToken)
        {
            var result = await _medicalServiceService.CreateMedicalService(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("CreateRadiologyBodyType")]
        public async Task<IActionResult> CreateRadiologyBodyType(RadiologyBodyTypeRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _medicalServiceService.CreateRadiologyBodyType(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetMedicalServices([FromQuery] PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var result = await _medicalServiceService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetRadiologyBodyTypes")]
        public async Task<ErrorResponseModel<List<RadiologyBodyType>>> GetRadiologyBodyTypes()
        {
            var result = await _medicalServiceService.GetRadiologyBodyTypes();
            return result;
        }

        [HttpPost("GetMedicalService")]
        public async Task<IActionResult> GetMedicalService(PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var result = await _medicalServiceService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalService([FromRoute] int id, [FromBody] MedicalServiceRequest request, CancellationToken cancellationToken)
        {
            var result = await _medicalServiceService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }

    }
}
