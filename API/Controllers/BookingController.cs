using Application.Features.Booking.Commands;
using Application.Features.Booking.Queries;
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

        [HttpPost("select-seat")]
        public async Task<IActionResult> SelectSeat([FromBody] SelectSeatCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("passenger-data")]
        public async Task<IActionResult> RegisterPassenger([FromBody] RegisterPassengerCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetSeatByIdOfVehicle/")]
        public async Task<IActionResult> GetSeatByIdOfVehicle([FromQuery] GetSeatByIdOfVehicleQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpGet("GetPersonDataByDni")]
        public async Task<IActionResult> GetPersonDataByDni([FromQuery] GetPersonDataByDniQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("RegisterBooking")]
        public async Task<IActionResult> RegisterBooking([FromBody] RegisterBookigCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) { 
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
