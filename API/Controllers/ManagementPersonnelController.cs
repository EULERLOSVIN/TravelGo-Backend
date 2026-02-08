using Application.DTOs;
using Application.Features.ManagementPersonnel.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementPersonnelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManagementPersonnelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetPersonnel")]
        public async Task<IActionResult> GetAllManagementPersonnel([FromQuery] FilterPersonnelDto queryDto)
        {
            var query = new GetPersonnelByFlterQuery(queryDto);
            var managementPersonnel = await _mediator.Send(query);

            return Ok(managementPersonnel);
        }

        [HttpGet("GetPersonnelFormRequirements")]
        public async Task<IActionResult> GetPersonnelFormRequirements()
        {
            var query = new GetPersonnelFormRequirementsQuery();
            var managementPersonnel = await _mediator.Send(query);

            return Ok(managementPersonnel);
        }
    }
}
