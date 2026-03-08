using Application.DTOs.ManageSales;
using Application.Features.ManageSales.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageSalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManageSalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetSalesMade")]
        public async Task<IActionResult> GetSalesMade([FromQuery] FilterOfManageSalesDto filters)
        {
            var query = new GetSalesByFiltersQuery(filters ?? new FilterOfManageSalesDto());
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("GetFiltersForManageSales")]
        public async Task<IActionResult> GetFiltersForManageSales()
        {
            var query = new GetFiltersForMageSalesQuery();
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}