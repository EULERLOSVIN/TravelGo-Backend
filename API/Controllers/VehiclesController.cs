using Application.DTOs.vehicles;
using Application.DTOs.Vehicles;
using Application.Features.vehicles.Comands;
using Application.Features.vehicles.Queries;
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

    [HttpGet("GetSummaryStatisticalOfVehicles")]
    public async Task<IActionResult> GetSummaryStatisticalOfVehicles()
    {
        var result = await _mediator.Send(new GetStatisticalSummaryOfVehiclesQuery());
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut("EditVehicle")]
    public async Task<IActionResult> EditVehicle([FromBody] EditVehicleDto newData)
    {
        var result = await _mediator.Send(new EditVehicleCommand(newData));
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
