using Domain.Entities;
﻿


using Application.DTOs.ManageSales;
using Application.Interfaces.ManageSales;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.ManageSales
{
    public class GetFilterRepository: IGetFilterRepository
    {
        private readonly ApplicationDbContext _context;
        public GetFilterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FiltersDto> GetFilterForMageSales()
        {
            var ticketStates = await _context.TicketStates.AsNoTracking().ToListAsync();
            var routes = await _context.TravelRoutes.AsNoTracking().ToListAsync();

            return new FiltersDto
            {
                // Mapeo de Estados
                StateTickets = ticketStates.Select(s => new StateTicketDto
                {
                    IdState = s.IdTicketState,
                    Name = s.Name
                }).ToList(),

                // Mapeo de Rutas
                Routes = routes.Select(r => new RouteDto
                {
                    IdRoute = r.IdTravelRoute,
                    Name = r.NameRoute
                }).ToList()
            };
        }
    }
}
