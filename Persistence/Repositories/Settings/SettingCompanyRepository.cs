using Domain.Entities;
﻿using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Settings;
using Application.DTOs.Settings;

namespace Persistence.Repositories.Settings
{
    public class SettingCompanyRepository : ISettingCompanyRepository
    {
        public readonly ApplicationDbContext _context;
        public SettingCompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SettingCompany> GetCompanyAsync()
        {
            var company = await _context.Companies.FirstOrDefaultAsync();
            if (company == null) return null;

            return new SettingCompany
            {
                IdCompany = company.IdCompany,
                BusinessName = company.BusinessName,
                Ruc = company.Ruc,
                FiscalAddress = company.FiscalAddress,
                Phone = company.Phone,
                Email = company.Email
            };
        }

        public async Task<bool> UpdateCompanyAsync(SettingCompany dto)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.IdCompany == dto.IdCompany);
            
            // Si el ID no coincide, intentar jalar el primero disponible (solo si hay uno solo)
            if (company == null)
            {
                company = await _context.Companies.FirstOrDefaultAsync();
            }

            if (company == null) return false;

            company.BusinessName = dto.BusinessName;
            company.Ruc = dto.Ruc;
            company.FiscalAddress = dto.FiscalAddress;
            company.Phone = dto.Phone;
            company.Email = dto.Email;

            _context.Companies.Update(company);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
