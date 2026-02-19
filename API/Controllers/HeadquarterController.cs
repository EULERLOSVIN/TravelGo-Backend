using Application.DTOs.Headquarters;
using Application.Features.Headquarters.Commands;
using Application.Features.Headquarters.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeadquarterController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HeadquarterController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllHeadquartersQuery());
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetHeadquarterByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateHeadquarterDto dto)
        {
            try 
            {
                var result = await _mediator.Send(new CreateHeadquarterCommand(dto));
                if (!result) return BadRequest("No se pudo crear la sede. Verifique que el IdCompany y IdStateHeadquarter existan.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Devolvemos el mensaje de la excepción directamente (ej. "Ya existe una sede...")
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateHeadquarterDto dto)
        {
            // Validamos que el ID de la URL coincida con el ID del cuerpo
            if (id != dto.IdHeadquarter) return BadRequest("El ID de la URL no coincide con el ID del cuerpo.");

            var result = await _mediator.Send(new UpdateHeadquarterCommand(id, dto));

            // Si el resultado es false, asumimos que no se encontró o hubo error. 
            // En PlaceController usan NotFound(), así que seguiremos ese patrón.
            if (!result) return NotFound();

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteHeadquarterCommand(id));
            if (!result) return NotFound();
            return Ok(result);
        }
    }


}
