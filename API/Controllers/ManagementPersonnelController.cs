using Application.DTOs;
using Application.Features.ManagementPersonnel.Commands;
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

        [HttpGet("GetStatsPersonnel")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetStatsPersonnelQuery();
            var managementPersonnel = await _mediator.Send(query);

            return Ok(managementPersonnel);
        }

        [HttpGet("GetUserByIdAccount/{id}")]
        public async Task<IActionResult> GetUserByIdAccount(int id)
        {
            try
            {
                var query = new GetUserByIdAccountQuery(id);
                var user = await _mediator.Send(query);

                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message,
                    idSearch = id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error inesperado en el servidor.",
                    details = ex.Message
                });
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] EditPersonnelDto newData)
        {
            var command = new EditUserCommand(newData);
            var userUpdated = await _mediator.Send(command);
            return Ok(userUpdated);

        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var command = new DeleteUserCommand(id);
            var userDeleted = await _mediator.Send(command);
            return Ok(userDeleted);
        }

    }
}
