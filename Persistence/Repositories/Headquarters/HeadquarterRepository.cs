using Domain.Entities;
﻿
using System;
using System.Collections.Generic;
using System.Text;

using Application.DTOs.Headquarters;
using Application.Interfaces.Headquarters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context; // Tu DbContext
using Persistence; // Tus Entidades

namespace Persistence.Repositories.Headquarters
{
    public class HeadquarterRepository : IHeadquarterRepository
    {
        private readonly ApplicationDbContext _context;

        public HeadquarterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HeadquarterDto>> GetAllAsync()
        {
            return await _context.Headquarters
                .AsNoTracking()
                .Select(h => new HeadquarterDto
                {
                    IdHeadquarter = h.IdHeadquarter,
                    Name = h.Name,
                    Address = h.Address,
                    Department = h.Department,
                    Province = h.Province,
                    District = h.District,
                    Phone = h.Phone,
                    Email = h.Email,
                    IsMain = h.IsMain,
                    RegistrationDate = h.RegistrationDate,
                    StateHeadquarter = h.IdStateHeadquarterNavigation != null ? h.IdStateHeadquarterNavigation.Name : "Desconocido"
                })
                .ToListAsync();
        }

        public async Task<HeadquarterDto?> GetByIdAsync(int id)
        {
            return await _context.Headquarters
                .AsNoTracking()
                .Where(x => x.IdHeadquarter == id)
                .Select(h => new HeadquarterDto
                {
                    IdHeadquarter = h.IdHeadquarter,
                    Name = h.Name,
                    Address = h.Address,
                    Department = h.Department,
                    Province = h.Province,
                    District = h.District,
                    Phone = h.Phone,
                    Email = h.Email,
                    IsMain = h.IsMain,
                    RegistrationDate = h.RegistrationDate,
                    StateHeadquarter = h.IdStateHeadquarterNavigation != null ? h.IdStateHeadquarterNavigation.Name : "Desconocido"
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(CreateHeadquarterDto dto)
        {
            // 1. Validar que el DTO no sea nulo
            if (dto == null) return false;

            // 2. Validar que el nombre no esté vacío (Casillas vacias)
            if (string.IsNullOrWhiteSpace(dto.Name)) return false;

            // 3. Validar que no exista otra sede con el mismo nombre (Duplicados)
            bool exists = await _context.Headquarters.AnyAsync(x => x.Name == dto.Name);
            if (exists) return false;

            var entity = new Headquarter
            {
                IdCompany = dto.IdCompany,
                IdStateHeadquarter = dto.IdStateHeadquarter,
                Name = dto.Name,
                Address = dto.Address,
                Department = dto.Department,
                Province = dto.Province,
                District = dto.District,
                Phone = dto.Phone,
                Email = dto.Email,
                IsMain = dto.IsMain,
                RegistrationDate = DateTime.Now
            };

            _context.Headquarters.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(int id, UpdateHeadquarterDto dto)
        {
            var entity = await _context.Headquarters.FindAsync(id);
            if (entity == null) return false;

            entity.IdCompany = dto.IdCompany;
            entity.IdStateHeadquarter = dto.IdStateHeadquarter;
            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.Department = dto.Department;
            entity.Province = dto.Province;
            entity.District = dto.District;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.IsMain = dto.IsMain;

            _context.Headquarters.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Headquarters.FindAsync(id);
            if (entity == null) return false;

            _context.Headquarters.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Headquarters.AnyAsync(h => h.Name == name);
        }
    }
}
