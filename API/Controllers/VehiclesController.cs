using Application.Features.vehicles.Queries;
using Application.Features.Vehicles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: /api/vehicles
    [HttpGet]
    public async Task<IActionResult> GetVehicles()
    {
        var result = await _mediator.Send(new GetVehiclesQuery());
        return Ok(result);
    }

    // GET: /api/vehicles/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _mediator.Send(new GetVehicleSummaryQuery());
        return Ok(result);
    }

    [HttpGet("GetDrivers")]
    public async Task<IActionResult> GetDrivers()
    {
        var result = await _mediator.Send(new GetAllDriverQuery());
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
