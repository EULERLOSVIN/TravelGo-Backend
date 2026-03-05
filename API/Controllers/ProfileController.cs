using Application.DTOs.Settings;
using Application.Interfaces.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protegemos el endpoint para que solo gente logueada acceda
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        // GET: api/Profile/5
        [HttpGet("{idAccount}")]
        public async Task<IActionResult> GetProfile(int idAccount)
        {
            var profile = await _profileRepository.GetProfileAsync(idAccount);
            if (profile == null)
            {
                return NotFound(new { message = "Perfil no encontrado" });
            }
            return Ok(profile);
        }

        // PUT: api/Profile/5
        [HttpPut("{idAccount}")]
        public async Task<IActionResult> UpdateProfile(int idAccount, [FromBody] ProfileDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _profileRepository.UpdateProfileAsync(idAccount, profileDto);
            if (!success)
            {
                return NotFound(new { message = "Cuenta o perfil no encontrado para actualizar." });
            }

            return Ok(new { message = "Perfil actualizado exitosamente." });
        }
    }
}
