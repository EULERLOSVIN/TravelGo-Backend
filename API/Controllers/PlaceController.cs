// places=darwin
using Application.DTOs;
using Application.Features.Places.Commands;
using Application.Features.Places.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PlaceController(IMediator mediator) => _mediator = mediator;

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddPlaceDto dto)
        {
            var result = await _mediator.Send(new AddPlaceCommand(dto));
            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPlacesQuery());
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePlaceDto dto)
        {
            var result = await _mediator.Send(new UpdatePlaceCommand(dto));
            if (!result) return NotFound();
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePlaceCommand(id));
            if (!result) return NotFound();
            return Ok(result);
        }
    }
}
