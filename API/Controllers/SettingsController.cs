using Application.DTOs.Settings;
using Application.Interfaces.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingCompanyRepository _repository;

        public SettingsController(ISettingCompanyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("Company")]
        public async Task<IActionResult> GetCompany()
        {
            var company = await _repository.GetCompanyAsync();
            if (company == null)
            {
                return NotFound(new { message = "No company settings found." });
            }

            return Ok(company);
        }

        [HttpPut("Company")]
        public async Task<IActionResult> UpdateCompany([FromBody] SettingCompany dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Always enforce updating the main company (Id = 1) if we assume a single-tenant or global config
            dto.IdCompany = 1; 

            var success = await _repository.UpdateCompanyAsync(dto);
            if (!success)
            {
                return StatusCode(500, new { message = "An error occurred while updating the company settings." });
            }

            return Ok(new { message = "Company settings updated successfully." });
        }
    }
}
