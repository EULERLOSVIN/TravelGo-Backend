using Domain.Entities;
﻿

using Application.DTOs.ManageSales;
using Application.Interfaces.ManageSales;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.ManageSales
{
    public class GetSalesRepository: IGetSalesRepository
    {
        public readonly ApplicationDbContext _context;
        public GetSalesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SaleDto>?> GetSalesByFilters(FilterOfManageSalesDto filters)
        {
            filters ??= new FilterOfManageSalesDto();

            var query = _context.TravelTickets
                .AsNoTracking()
                .AsQueryable();

            // 1. Filtros (Sin cambios, tu lógica es correcta)
            if (filters.FromDate.HasValue)
                query = query.Where(t => t.TravelDate >= filters.FromDate);
            if (filters.UntilDate.HasValue)
                query = query.Where(t => t.TravelDate <= filters.UntilDate);
            if (filters.IdRoute.HasValue)
                query = query.Where(t => t.IdTravelRoute == filters.IdRoute);
            if (filters.StateTicket.HasValue)
                query = query.Where(t => t.IdTicketState == filters.StateTicket.Value);

            // 2. Paginación Robusta
            int pageSize = 15;
            // Aseguramos que la página sea siempre al menos 1
            int pageNumber = (filters.page != null && filters.page > 0) ? filters.page.Value : 1;

            // 3. Ordenamiento Estable (CRÍTICO)
            // Ordenamos por Fecha y luego por ID para romper empates y evitar saltos fantasma
            query = query
                .OrderByDescending(t => t.TravelDate)
                .ThenByDescending(t => t.IdTravelTicket)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // 4. Proyección
            return await query.Select(t => new SaleDto
            {
                IdTicket = t.IdTravelTicket,
                Date = t.TravelDate,
                FirstName = t.IdBillingNavigation.IdPersonNavigation.FirstName ?? "Sin nombre",
                LastName = t.IdBillingNavigation.IdPersonNavigation.LastName ?? "Sin apellido",
                Dni = t.IdBillingNavigation.IdPersonNavigation.NumberIdentityDocument ?? "Sin DNI",
                Route = t.IdTravelRouteNavigation.NameRoute,
                SeatNumber = t.SeatNumber,
                UnitPrice = t.IdTravelRouteNavigation.Price,
                PaymentMethod = t.IdBillingNavigation.IdPaymentMethodNavigation.Name,
            }).ToListAsync();
        }
    }
}
