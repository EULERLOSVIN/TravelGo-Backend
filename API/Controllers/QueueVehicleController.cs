using Application.Common;
using Application.DTOs.QueueVehicles;
using Application.Features.QueueVehicles.Commands;
using Application.Features.QueueVehicles.Commands.DispatchVehicle;
using Application.Features.QueueVehicles.Queries;
using Application.Features.QueueVehicles.Queries.GetDriverQueueInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueVehicleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QueueVehicleController(IMediator mediator) => _mediator = mediator;

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddQueueVehicleDto dto)
        {
            var result = await _mediator.Send(new AddQueueVehicleCommand(dto));
            return Ok(result);
        }

        // Now filters by idRoute (direct FK that exists in AssignQueue)
        // Added isArrival support to show incoming vehicles
        // Now filters by both idHeadquarter and idRoute for strict isolation
        // Added isArrival support to show incoming vehicles for this specific HQ
        [HttpGet("getActiveQueue/{idHeadquarter}/{idRoute}")]
        public async Task<IActionResult> GetActiveQueue(int idHeadquarter, int idRoute, [FromQuery] bool isArrival = false)
        {
            var result = await _mediator.Send(new GetActiveQueueQuery(idHeadquarter, idRoute, isArrival));
            return Ok(result);
        }

        [HttpPost("dispatch/{idAssignQueue}")]
        public async Task<IActionResult> Dispatch(int idAssignQueue)
        {
            var result = await _mediator.Send(new DispatchVehicleCommand(idAssignQueue));
            return Ok(result);
        }

        [HttpGet("getDriverQueueInfo/{dni}")]
        public async Task<IActionResult> GetDriverQueueInfo(string dni)
        {
            var result = await _mediator.Send(new GetDriverQueueInfoQuery(dni));
            return Ok(result);
        }

        [HttpPut("updateRoute")]
        public async Task<IActionResult> UpdateRoute([FromBody] UpdateRouteQueueVehicleDto dto)
        {
            var result = await _mediator.Send(new UpdateQueueVehicleRouteCommand(dto));
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteQueueVehicleCommand(id));
            return Ok(result);
        }

        [HttpPost("registerArrival")]
        public async Task<IActionResult> RegisterArrival([FromBody] RegisterArrivalDto dto)
        {
            var result = await _mediator.Send(new RegisterArrivalCommand(dto.DriverDni));
            return Ok(result);
        }
    }

    public record RegisterArrivalDto(string DriverDni);
}
