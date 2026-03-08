using Domain.Entities;
﻿
using Application.DTOs.Customers;
using Application.Interfaces.Customers;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Customers
{
    public class GetAllPlaceofRouteRepository: IGetAllPlaceofRouteRepository
    {
        private readonly ApplicationDbContext _context;

        public GetAllPlaceofRouteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task <List<GetPlaceDto>?> GetAllPlaceofRoute()
        {
            var placeofroute = await _context.Places.Where(
                r => _context.TravelRoutes.Any(tr => tr.IdPlaceA == r.IdPlace || tr.IdPlaceB == r.IdPlace)
                ).Select(p => new GetPlaceDto
                {
                    Id = p.IdPlace,
                    Name = p.Name
                })
                .ToListAsync();
            if (placeofroute == null) return null;
            return placeofroute;
        }
    }
}
