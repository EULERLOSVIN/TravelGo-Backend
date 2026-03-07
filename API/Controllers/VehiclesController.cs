using Application.DTOs.vehicles;
using Application.DTOs.Vehicles;
using Application.Features.vehicles.Comands;
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

    [HttpGet("summaryStaticsVehicles")]
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

    [HttpGet("GetAllStateVehicles")]
    public async Task<IActionResult> GetAllStateVehicles()
    {
        var result = await _mediator.Send(new GetAllStatesVehicleQuery());
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("RegisterVehicle")]
    public async Task<IActionResult> RegisterVehicle([FromBody] CreateVehicleDto dto)
    {
        var result = await _mediator.Send(new RegisterVehicleCommand(dto));
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("GetVehiclesByFilter")]
    public async Task<IActionResult> GetVehiclesByFilter([FromQuery] FilterVehicleDto Filters)
    {
        var result = await _mediator.Send(new GetVehiclesByFiltersQuery(Filters));
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
