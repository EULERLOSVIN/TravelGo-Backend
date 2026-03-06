using Application.DTOs.QueueVehicles;
using Application.Features.QueueVehicles.Commands;
using Application.Features.QueueVehicles.Queries;
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

        [HttpGet("getActiveQueue/{idHeadquarter}")]
        public async Task<IActionResult> GetActiveQueue(int idHeadquarter)
        {
            var result = await _mediator.Send(new GetActiveQueueQuery(idHeadquarter));
            return Ok(result);
        }

        [HttpGet("getDriverQueueInfo/{dni}")]
        public async Task<IActionResult> GetDriverQueueInfo(string dni)
        {
            var result = await _mediator.Send(new Application.Features.QueueVehicles.Queries.GetDriverQueueInfo.GetDriverQueueInfoQuery(dni));
            if (result == null) return NotFound("Chofer no encontrado.");
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
            if (!result) return NotFound();
            return Ok(result);
        }
    }
}
