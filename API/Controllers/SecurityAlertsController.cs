using Application.Features.SecurityAlerts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecurityAlertsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SecurityAlertsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetExpiringDocuments")]
    public async Task<IActionResult> GetExpiringDocuments()
    {
        var result = await _mediator.Send(new GetSecurityAlertsQuery());
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
