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
            // 1. Get active routes to calculate HasRoutes correctly
            var routePlaces = await _context.TravelRoutes
                .AsNoTracking()
                .Where(r => r.IsActive)
                .Select(r => new { r.IdPlaceA, r.IdPlaceB })
                .ToListAsync();

            var activePlaceIds = routePlaces.Select(r => r.IdPlaceA)
                .Union(routePlaces.Select(r => r.IdPlaceB))
                .Distinct()
                .ToList();

            var placesPool = activePlaceIds.Any() 
                ? await _context.Places.AsNoTracking().Where(p => activePlaceIds.Contains(p.IdPlace)).ToListAsync()
                : new List<Place>();

            // 2. Get all headquarters without filtering
            var allHeadquarters = await _context.Headquarters
                .Include(h => h.IdStateHeadquarterNavigation)
                .AsNoTracking()
                .ToListAsync();

            var result = new List<HeadquarterDto>();
            foreach (var h in allHeadquarters)
            {
                // Check matching places for HasRoutes logic
                var matchingActivePlaces = GetMatchingPlaceIds(h, placesPool);
                
                result.Add(new HeadquarterDto
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
                    StateHeadquarter = h.IdStateHeadquarterNavigation?.Name ?? "Desconocido",
                    HasRoutes = matchingActivePlaces.Any()
                });
            }
            return result;
        }

        public async Task<HeadquarterDto?> GetByIdAsync(int id)
        {
            var h = await _context.Headquarters
                .Include(h => h.IdStateHeadquarterNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdHeadquarter == id);

            if (h == null) return null;

            var places = await _context.Places.AsNoTracking().ToListAsync();
            var hqPlaces = GetMatchingPlaceIds(h, places);
            
            bool hasRoutes = false;
            if (hqPlaces.Any())
            {
                hasRoutes = await _context.TravelRoutes.AsNoTracking().AnyAsync(r => 
                    r.IsActive && 
                    (hqPlaces.Contains(r.IdPlaceA) || hqPlaces.Contains(r.IdPlaceB)) &&
                    _context.RouteAssignments.Any(ra => ra.IdTravelRoute == r.IdTravelRoute));
            }

            return new HeadquarterDto
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
                StateHeadquarter = h.IdStateHeadquarterNavigation?.Name ?? "Desconocido",
                HasRoutes = hasRoutes
            };
        }

        private List<int> GetMatchingPlaceIds(Headquarter h, List<Place> places)
        {
            string Normalize(string text)
            {
                if (string.IsNullOrWhiteSpace(text)) return "";
                
                // Lowercase and common cleanups
                var normalized = text.ToLower()
                    .Replace("sede", "").Replace("central", "").Replace("principal", "").Replace("prinsipal", "")
                    .TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ').Trim();

                if (string.IsNullOrEmpty(normalized)) return "";

                // Advanced accent removal using FormD
                string formD = normalized.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (char c in formD)
                {
                    var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                    if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString().Normalize(NormalizationForm.FormC);
            }

            var nName = Normalize(h.Name);
            var nDist = Normalize(h.District);
            var nProv = Normalize(h.Province);

            if (string.IsNullOrEmpty(nName) && string.IsNullOrEmpty(nDist) && string.IsNullOrEmpty(nProv))
            {
                return new List<int>();
            }

            return places
                .AsEnumerable() // Move to memory for complex matching if needed, though here we match against many IDs
                .Where(p => {
                    var pName = Normalize(p.Name);
                    if (string.IsNullOrEmpty(pName)) return false;

                    // Priority matching: 
                    // 1. Exact match on District/Province is strongest (if not empty)
                    // 2. Exact match on Name (after removing "Sede")
                    if (nDist != "" && pName == nDist) return true;
                    if (nProv != "" && pName == nProv) return true;
                    if (nName != "" && pName == nName) return true;

                    return false;
                })
                .Select(p => p.IdPlace)
                .ToList();
        }

        public async Task<bool> CreateAsync(CreateHeadquarterDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new Exception("El nombre de la sede es obligatorio.");

            // 1. Validar duplicados por nombre
            if (await _context.Headquarters.AnyAsync(x => x.Name == dto.Name))
                throw new Exception($"Ya existe una sede registrada con el nombre '{dto.Name}'.");

            // 2. Validar IdCompany
            int validCompanyId = dto.IdCompany;
            var companyExists = await _context.Companies.AnyAsync(c => c.IdCompany == validCompanyId);
            
            if (!companyExists)
            {
                // Intentar recuperar la primera empresa disponible
                var firstCompany = await _context.Companies.FirstOrDefaultAsync();
                if (firstCompany == null)
                {
                    throw new Exception("No existe ninguna Empresa (Company) registrada en la base de datos. Debe crear una empresa primero.");
                }
                validCompanyId = firstCompany.IdCompany;
            }

            // 3. Validar IdStateHeadquarter
            int validStateId = dto.IdStateHeadquarter;
            var stateExists = await _context.StateHeadquarters.AnyAsync(s => s.IdStateHeadquarter == validStateId);
            
            if (!stateExists)
            {
                // Intentar recuperar el primer estado disponible o usar 1 si la tabla está vacía (pero fallará el FK si está vacía)
                var firstState = await _context.StateHeadquarters.FirstOrDefaultAsync();
                if (firstState == null)
                {
                    throw new Exception("No existen Estados de Sede (StateHeadquarter) registrados. Verifique la base de datos.");
                }
                validStateId = firstState.IdStateHeadquarter;
            }

            var entity = new Headquarter
            {
                IdCompany = validCompanyId,
                IdStateHeadquarter = validStateId,
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
            var saved = await _context.SaveChangesAsync();
            
            if (saved <= 0)
            {
                throw new Exception("Error interno al guardar la sede en la base de datos.");
            }
            
            return true;
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
