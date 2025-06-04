using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.StoreCount;
using Hospital_MS.Interfaces.HMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers;

[Authorize]
public class StoreCountsController(IStoreCountService storeCountService) : ApiBaseController
{
    private readonly IStoreCountService _storeCountService = storeCountService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StoreCountRequest request,CancellationToken cancellationToken)     
    {
        var result = await _storeCountService.CreateAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] PagingFilterModel pagingFilter,CancellationToken cancellationToken)   
    {
        var result = await _storeCountService.GetAllAsync(pagingFilter, cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById( int id, CancellationToken cancellationToken) 
    {
        var result = await _storeCountService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update( int id,[FromBody] StoreCountRequest request,CancellationToken cancellationToken)   
    {
        var result = await _storeCountService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }
 
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)    
    {
        var result = await _storeCountService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }
}