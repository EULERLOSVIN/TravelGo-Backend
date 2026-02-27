

using Application.DTOs.vehicles;
using Application.Interfaces.vehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.vehicles
{
    public class GetAllDriverRepository: IGetAllDriverRepository
    {
        private readonly ApplicationDbContext _context;

        public GetAllDriverRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<List<PersonDto>> GetDriversAsync()
        {
            var personas = await _context.People
                .Where(p => p.Accounts.Any(a =>
                    a.IdRoleNavigation.IdRole == 3 &&
                    a.IdStateAccount == 1))
                .Select(p => new PersonDto
                {
                    IdPerson = p.IdPerson,
                    Name = p.FirstName
                })
                .ToListAsync();

            return personas;
        }
    }
} 
