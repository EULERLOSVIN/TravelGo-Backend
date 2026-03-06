using Domain.Entities;
﻿using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetStatesAccountRepository: IGetStatesAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public GetStatesAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StateOfAccountDto>> GetStatesAccount()
        {
            var states = await _context.StateAccounts.ToListAsync();
            return states.Select(x => new StateOfAccountDto { Id = x.IdStateAccount, Name = x.Name }).ToList();
        }
    }
}
