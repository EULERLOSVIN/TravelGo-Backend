using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetPersonnelRepository: IGetPersonnelRepository
    {
        private readonly ApplicationDbContext _context;

        public GetPersonnelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PersonnelDto>> GetPersonnelByFilters(FilterPersonnelDto filters)
        {
            int pageNumber = filters.PageNumber ?? 1;
            int pageSize = 15;

            var query = _context.Accounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
            {
                query = query.Where(c =>
                    c.IdPersonNavigation.FirstName.Contains(filters.SearchTerm) ||
                    c.IdPersonNavigation.LastName.Contains(filters.SearchTerm) ||
                    c.IdPersonNavigation.NumberIdentityDocument.Contains(filters.SearchTerm));
            }

            var listPersonnel = await query
                .OrderBy(c => c.IdPersonNavigation.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new PersonnelDto
                {
                    Id = c.IdAccount,
                    FirstName = c.IdPersonNavigation.FirstName,
                    LastName = c.IdPersonNavigation.LastName,
                    Role = c.IdRoleNavigation.Name,
                    Dni = c.IdPersonNavigation.NumberIdentityDocument,
                    PhoneNumber = c.IdPersonNavigation.PhoneNumber,
                    Email = c.Email
                })
                .ToListAsync();

            return listPersonnel;
        }



    }
}
