using Microsoft.AspNetCore.Mvc;
using Persistence.Context;
using Persistence;
using Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            // 1. Seed Company
            if (!_context.Companies.Any())
            {
                _context.Companies.Add(new Company
                {
                    BusinessName = "TravelGo Default Company",
                    Ruc = "20000000001",
                    // Address property does not exist in Company entity
                    Phone = "000-000000",
                    Email = "admin@travelgo.com",
                    FiscalAddress = "Fiscal Address 123",
                    RegistrationDate = DateTime.Now
                });
            }

            // 2. Seed StateHeadquarter
            if (!_context.StateHeadquarters.Any())
            {
                _context.StateHeadquarters.AddRange(
                    new StateHeadquarter { Name = "Activa" },
                    new StateHeadquarter { Name = "Inactiva" }
                );
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Database seeded successfully." });
        }
    }
}
