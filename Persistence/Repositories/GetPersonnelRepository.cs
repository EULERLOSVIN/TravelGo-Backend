using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetPersonnelRepository : IGetPersonnelRepository
    {
        private readonly ApplicationDbContext _context;

        public GetPersonnelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PersonnelDto>> GetPersonnelByFilters(FilterPersonnelDto filters)
        {
            const int pageSize = 10;
            int pageNumber = filters.PageNumber ?? 1;
            if (pageNumber < 1) pageNumber = 1;
            var query = _context.Accounts
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
            {
                var term = filters.SearchTerm.Trim();
                query = query.Where(c =>
                    c.IdPersonNavigation.FirstName.Contains(term) ||
                    c.IdPersonNavigation.LastName.Contains(term) ||
                    c.IdPersonNavigation.NumberIdentityDocument.Contains(term));
            }

            return await query
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
                    Email = c.Email,
                    State = c.IdStateAccountNavigation.Name,
                    TypeDocument = c.IdPersonNavigation.IdTypeDocumentNavigation.Name
                })
                .ToListAsync();
        }
    }
}
