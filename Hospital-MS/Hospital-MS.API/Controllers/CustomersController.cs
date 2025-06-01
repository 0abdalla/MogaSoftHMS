using Hospital_MS.API.Controllers;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Customers;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class CustomersController : ApiBaseController
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CustomerRequest request)
    {
        var result = await _customerService.CreateAsync(request);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel filter)
    {
        var result = await _customerService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerRequest request)
    {
        var result = await _customerService.UpdateAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteAsync(id);
        return Ok(result);
    }
}