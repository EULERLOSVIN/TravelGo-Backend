using Application.Features.Driver.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DriverController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetTripsMadeByDriver")]
        public async Task<IActionResult> GetTripMadeByDriver([FromQuery] GetTripsMadeQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetStartingOrderOfDriver")]
        public async Task<IActionResult> GetStartingOrderOfDriver([FromQuery] GetStartingOrderOfDriverQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetStatisticsSummaryOfTrips")]
        public async Task<IActionResult> GetDriverById([FromQuery] GetCompletedRacesQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
