
using Application.DTOs;
using Application.Features.TravelRoutes.Commands;
using Application.Features.TravelRoutes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TravelRouteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TravelRouteController(IMediator mediator) => _mediator = mediator;

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddTravelRouteDto dto)
        {
            var result = await _mediator.Send(new AddTravelRouteCommand(dto));
            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTravelRoutesQuery());
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTravelRouteDto dto)
        {
            var result = await _mediator.Send(new UpdateTravelRouteCommand(dto));
            if (!result) return NotFound();
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTravelRouteCommand(id));
            if (!result) return NotFound();
            return Ok(result);
        }

        [HttpGet("getPlacesOfRoutes")]
        public async Task<IActionResult> GetPlacesOfRoutes()
        {
            var result = await _mediator.Send(new GetAllPlaceOfRouteQuery());
            if (result.IsSuccess)
            {
                return Ok(result);
            }
           return BadRequest(result);
        }
    }
}
