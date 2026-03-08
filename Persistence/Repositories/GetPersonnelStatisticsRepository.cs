using Domain.Entities;
﻿

using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetPersonnelStatisticsRepository: IGetPersonnelStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public GetPersonnelStatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StatsUsersDto> GetStatsUsers()
        {
            var stats = await _context.Accounts
                .GroupBy(_ => 1)
                .Select(g => new StatsUsersDto
                {
                    TotalUsers = g.Count(),
                    UsersActive = g.Count(x => x.IdStateAccount == 1),
                    UsersInactive = g.Count(x => x.IdStateAccount == 2)
                })
                .FirstOrDefaultAsync() ?? new StatsUsersDto();

            return stats;
        }
    }
}
