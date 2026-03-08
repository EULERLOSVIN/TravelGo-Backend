using Domain.Entities;
﻿using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetUserRepository: IGetUserRepository
    {
        private readonly ApplicationDbContext _context;
        public GetUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PersonnelDto> GetPersonnelByIdAccount(int idAccount) 
        {
            var personnel = await _context.Accounts
            .Include(p => p.IdPersonNavigation)
            .Include(p => p.IdRoleNavigation)
            .Include(p => p.IdStateAccountNavigation)
            .FirstOrDefaultAsync(p => p.IdAccount == idAccount);

            if (personnel == null)
            {
                throw new KeyNotFoundException($"No se encontró personal asociado a la cuenta con ID: {idAccount}");
            }

            return new PersonnelDto
            {
                Id = personnel.IdAccount,
                FirstName = personnel.IdPersonNavigation?.FirstName ?? "N/A",
                LastName = personnel.IdPersonNavigation?.LastName ?? "N/A",
                Role = personnel.IdRoleNavigation?.Name ?? "Sin Rol",
                Dni = personnel.IdPersonNavigation?.NumberIdentityDocument ?? "N/A",
                PhoneNumber = personnel.IdPersonNavigation?.PhoneNumber ?? "N/A",
                Email = personnel.Email,
                State = personnel.IdStateAccountNavigation?.Name ?? "Desconocido"
            };
        }

    }
}
