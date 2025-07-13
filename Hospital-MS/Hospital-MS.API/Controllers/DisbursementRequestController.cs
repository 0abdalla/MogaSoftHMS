using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_MS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisbursementRequestController : ControllerBase
    {
        private readonly IDisbursementRequestService _disbursementRequestService;

        public DisbursementRequestController(IDisbursementRequestService disbursementRequestService)
        {
            _disbursementRequestService = disbursementRequestService;
        }

        [HttpPost]
        public async Task<ActionResult<DisbursementRequest>> CreateRequest(DisbursementRequest request)
        {
            try
            {
                // Check stock availability
                var isAvailable = await _disbursementRequestService.CheckStockAvailabilityAsync(
                    request.ItemCode, 
                    request.RequestedQuantity);

                if (!isAvailable)
                {
                    return BadRequest(new { 
                        message = "?????? ??? ????? ?????",
                        availableQuantity = await _disbursementRequestService.GetAvailableStockQuantityAsync(request.ItemCode)
                    });
                }

                var result = await _disbursementRequestService.CreateRequestAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisbursementRequest>>> GetAllRequests()
        {
            var requests = await _disbursementRequestService.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisbursementRequest>> GetRequest(int id)
        {
            var request = await _disbursementRequestService.GetRequestByIdAsync(id);
            if (request == null)
                return NotFound();

            return Ok(request);
        }

        [HttpPut("{id}/process")]
        public async Task<ActionResult<DisbursementRequest>> ProcessRequest(
            int id, 
            [FromBody] ProcessRequestModel model)
        {
            try
            {
                var result = await _disbursementRequestService.ProcessRequestAsync(
                    id, 
                    model.Status, 
                    model.ProcessedBy);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("stock/{itemCode}")]
        public async Task<ActionResult<decimal>> GetAvailableStock(string itemCode)
        {
            var quantity = await _disbursementRequestService.GetAvailableStockQuantityAsync(itemCode);
            return Ok(new { availableQuantity = quantity });
        }
    }

    public class ProcessRequestModel
    {
        public string Status { get; set; }
        public string ProcessedBy { get; set; }
    }
}