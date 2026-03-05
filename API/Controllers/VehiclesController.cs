using Application.DTOs.Vehicles;
using Application.Features.vehicles.Comands;
using Application.Features.vehicles.Queries;
using Application.Features.Vehicles.Commands;
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
    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleDto dto)
    {
        var result = await _mediator.Send(new CreateVehicleCommand(dto));

        if (!result)
            return BadRequest("No se pudo crear el vehículo.");

        return Ok(new { message = "Vehículo creado correctamente" });
    }

    [HttpPost("UpdateVehicle")]
    public async Task<IActionResult> UpdateVehicle(string unitId, [FromBody] CreateVehicleDto dto)
    {
        var result = await _mediator.Send(new UpdateVehicleCommand(unitId, dto));
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(new { message = "Vehículo actualizado correctamente" });
    }
    [HttpPost("DeleteVehicle")]
    public async Task<IActionResult> DeleteVehicle(string unitId)
    {
        var result = await _mediator.Send(new DeleteVehicleCommand(unitId));
        if (!result)
            return BadRequest("No se pudo eliminar el vehículo.");
        return Ok(new { message = "Vehículo eliminado correctamente" });
    }
}
