using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetAllRolesRepositoy : IGetAllRolesRepository
    {
        private readonly ApplicationDbContext _context;
        public GetAllRolesRepositoy(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleOfUserDto>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles.Select(x => new RoleOfUserDto { Id = x.IdRole, Name = x.Name }).ToList();
        }
    }
}
