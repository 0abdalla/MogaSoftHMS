﻿using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PriceQuotation;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hospital_MS.API.Controllers;
[Authorize]
[SwaggerTag("عرض الاسعار")]
public class PriceQuotationsController : ApiBaseController
{
    private readonly IPriceQuotationService _service;

    public PriceQuotationsController(IPriceQuotationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PriceQuotationRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PriceQuotationRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetByPurchaseRequest/{purchaseRequestId}")]
    public async Task<IActionResult> GetByPurchaseRequest(int purchaseRequestId, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllByPurchaseRequestIdAsync(purchaseRequestId, cancellationToken);
        return Ok(result);
    }

    [HttpPut("SubmitPriceQuotation/{purchaseRequestId}")]
    public async Task<IActionResult> PriceQuotation(int purchaseRequestId, CancellationToken cancellationToken)
    {
        var result = await _service.SubmitPriceQuotationByPurchaseRequestIdAsync(purchaseRequestId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("Approved")]
    public async Task<IActionResult> GetAllApproved(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllApprovedAsync(cancellationToken);
        return Ok(result);
    }
}
