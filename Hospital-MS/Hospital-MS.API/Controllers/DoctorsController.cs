﻿using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Doctors;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [Authorize]
    public class DoctorsController(IDoctorService doctorService) : ApiBaseController
    {
        private readonly IDoctorService _doctorService = doctorService;

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] DoctorRequest request, CancellationToken cancellationToken)
        {
            var result = await _doctorService.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetAllDoctors(PagingFilterModel pagingFilter, CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetAllAsync(pagingFilter, cancellationToken);
            return Ok(result);
        }

        [HttpGet("counts")]
        public async Task<IActionResult> GetAllCount(CancellationToken cancellationToken)
        {
            var result = await _doctorService.GetCountsAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] DoctorRequest request, CancellationToken cancellationToken)
        {
            var result = await _doctorService.UpdateAsync(id, request, cancellationToken);
            return Ok(result);
        }
    }
}
