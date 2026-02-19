using Application.Features.Booking.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ACCIÓN 1: Seleccionar Asiento (Imagen 3)
        // Cambia el estado de 'Disponible' a 'Reservado'
        [HttpPost("select-seat")]
        public async Task<IActionResult> SelectSeat([FromBody] SelectSeatCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // ACCIÓN 2: Registrar Pasajero (Imagen 4)
        // Mantiene el estado 'Reservado' pero completa la información
        [HttpPost("passenger-data")]
        public async Task<IActionResult> RegisterPassenger([FromBody] RegisterPassengerCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        // ACCIÓN 3: Confirmar Pago (Imagen 5)
        // Transiciona el estado de 'Reservado' a 'Vendido' (Confirmado)
        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
