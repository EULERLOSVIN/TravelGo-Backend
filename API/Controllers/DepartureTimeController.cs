using Application.DTOs.DepartureTimes;
using Application.Features.DepartureTimes.Commands;
using Application.Features.DepartureTimes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartureTimeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartureTimeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartureTime([FromBody] AddDepartureTimeDto dto)
        {
            var id = await _mediator.Send(new AddDepartureTimeCommand(dto));
            return Ok(new { Message = "Horario añadido exitosamente", Id = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartureTime(int id)
        {
            var result = await _mediator.Send(new DeleteDepartureTimeCommand(id));
            if (!result) return NotFound(new { Message = "Horario no encontrado" });
            return Ok(new { Message = "Horario eliminado correctamente" });
        }

        [HttpGet("route/{idTravelRoute}")]
        public async Task<IActionResult> GetByRoute(int idTravelRoute)
        {
            var result = await _mediator.Send(new GetDepartureTimesByRouteQuery(idTravelRoute));
            return Ok(result);
        }
    }
}
