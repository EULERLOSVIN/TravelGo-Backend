
using Application.DTOs;
using Application.Features.Booking.Queries;
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
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTravelRouteCommand(id));
            return Ok(result);
        }

        [HttpGet("getPlacesOfRoutes")]
        public async Task<IActionResult> GetPlacesOfRoutes()
        {
            var result = await _mediator.Send(new GetAllPlaceOfRouteQuery());
            return Ok(result);
        }

        [HttpGet("getRoutesByPlace")]
        public async Task<IActionResult> SerachRoutesByPlace([FromQuery] SearchRouteByPlaceQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("getRoutesByHeadquarter/{idHeadquarter}")]
        public async Task<IActionResult> GetRoutesByHeadquarter(int idHeadquarter, [FromQuery] string type = "departure")
        {
            var result = await _mediator.Send(new GetRoutesByHeadquarterQuery(idHeadquarter, type));
            return Ok(result);
        }

        //[HttpGet("GetAllRoutes")]
        //public async Task<IActionResult> GetAllRoutes()
        //{
        //    var result = await _mediator.Send(new GetAllRoutesQuery());

        //    if (result.IsSuccess)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}
    }
}
