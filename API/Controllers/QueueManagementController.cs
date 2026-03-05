using Application.Features.QueueManagement.Commands.AddQueueVehicle;
using Application.Features.QueueManagement.Commands.DeleteQueueVehicle;
using Application.Features.QueueManagement.Queries.GetActiveQueue;
using Application.Features.QueueManagement.Queries.GetDriverQueueInfo;
using Application.Interfaces.QueueManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueueManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public QueueManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("driver-info/{dni}")]
    public async Task<ActionResult<GetDriverQueueInfoResponse>> GetDriverInfo(string dni)
    {
        try
        {
            var result = await _mediator.Send(new GetDriverQueueInfoQuery { Dni = dni });
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<int>> AddQueueVehicle([FromBody] AddQueueVehicleCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<ActiveQueueDto>>> GetActiveQueue()
    {
        try
        {
            var result = await _mediator.Send(new GetActiveQueueQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("delete/{idQueueVehicle}")]
    public async Task<ActionResult<bool>> DeleteQueueVehicle(int idQueueVehicle)
    {
        try
        {
            var result = await _mediator.Send(new DeleteQueueVehicleCommand(idQueueVehicle));
            if (!result) return NotFound(new { message = "Unidad no encontrada en cola" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
