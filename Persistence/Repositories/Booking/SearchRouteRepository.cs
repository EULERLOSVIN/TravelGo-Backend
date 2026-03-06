using Domain.Entities;
﻿
using Application.DTOs.Customers;
using Application.Interfaces.Booking;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Booking
{
    public class SearchRouteRepository : ISearchRouteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IGetDepartureTimeRepository _getDepartureTimeRepository;
        public SearchRouteRepository(ApplicationDbContext context, IGetDepartureTimeRepository getDepartureTimeRepository)
        {
            _context = context;
            _getDepartureTimeRepository = getDepartureTimeRepository;
        }

        public async Task<DataRouteDto?> SearchRouteByPlaceToday(SearchTravelRouteDto searchTravelRouteDto)
        {
            var route = await _context.TravelRoutes
                .Include(r => r.RouteAssignments)
                    .ThenInclude(ra => ra.IdPersonNavigation)
                        .ThenInclude(p => p.Vehicles)
                            .ThenInclude(v => v.AssignQueues)
                                .ThenInclude(aq => aq.IdQueueVehicleNavigation)
                .Include(r => r.RouteAssignments)
                    .ThenInclude(ra => ra.IdPersonNavigation)
                        .ThenInclude(p => p.Vehicles)
                            .ThenInclude(v => v.SeatVehicles)
                .FirstOrDefaultAsync(r =>
                    (r.IdPlaceA == searchTravelRouteDto.IdPlaceOrigin && r.IdPlaceB == searchTravelRouteDto.IdPlaceDestination) ||
                    (r.IdPlaceA == searchTravelRouteDto.IdPlaceDestination && r.IdPlaceB == searchTravelRouteDto.IdPlaceOrigin)
                );

            if (route == null) return null;

            var vehiculosEnCola = route.RouteAssignments
                .Select(ra => ra.IdPersonNavigation)
                .SelectMany(p => p.Vehicles)
                .Where(v => v.AssignQueues.Any(aq => aq.IdQueueVehicleNavigation != null))
                .OrderBy(v => v.AssignQueues.First().IdQueueVehicleNavigation.Number)
                .ToList();

            var departureTimes = await _getDepartureTimeRepository.GetRemainingDepartureTimes(route.IdTravelRoute);

            var salidasProgramadas = new List<DepartureTimeByQueueDto>();

            for (int i = 0; i < departureTimes.Count; i++)
            {
                var horario = departureTimes[i];
                var vehiculo = i < vehiculosEnCola.Count ? vehiculosEnCola[i] : null;

                salidasProgramadas.Add(new DepartureTimeByQueueDto
                {
                    Id = horario.Id,
                    DepartureHour = DateTime.Today.Add(horario.Hour.ToTimeSpan()),
                    Vehicle = vehiculo != null ? new VehicleDto
                    {
                        Id = vehiculo.IdVehicle,
                        Photo = vehiculo.Photo != null ? Convert.ToBase64String(vehiculo.Photo) : null,
                        AvailableSeats = vehiculo.SeatVehicles.Count(sv => sv.StateSeat == true)
                    } : null
                });
            }

            return new DataRouteDto
            {
                IdRoute = route.IdTravelRoute,
                NameRoute = route.NameRoute,
                Price = (double)route.Price,
                DepartureHour = salidasProgramadas
            };
        }


        public async Task<DataRouteDto?> SearchRouteByPlaceTomorrow(SearchTravelRouteDto searchTravelRouteDto)
        {
            var route = await _context.TravelRoutes
                .Include(r => r.RouteAssignments)
                    .ThenInclude(ra => ra.IdPersonNavigation)
                        .ThenInclude(p => p.Vehicles)
                            .ThenInclude(v => v.AssignQueues)
                                .ThenInclude(aq => aq.IdQueueVehicleNavigation)
                .Include(r => r.RouteAssignments)
                    .ThenInclude(ra => ra.IdPersonNavigation)
                        .ThenInclude(p => p.Vehicles)
                            .ThenInclude(v => v.SeatVehicles)
                .FirstOrDefaultAsync(r =>
                    (r.IdPlaceA == searchTravelRouteDto.IdPlaceOrigin && r.IdPlaceB == searchTravelRouteDto.IdPlaceDestination) ||
                    (r.IdPlaceA == searchTravelRouteDto.IdPlaceDestination && r.IdPlaceB == searchTravelRouteDto.IdPlaceOrigin)
                );

            if (route == null) return null;

            var todosLosVehiculos = route.RouteAssignments
                .Select(ra => ra.IdPersonNavigation)
                .SelectMany(p => p.Vehicles)
                .Where(v => v.AssignQueues.Any(aq => aq.IdQueueVehicleNavigation != null))
                .OrderBy(v => v.AssignQueues.First().IdQueueVehicleNavigation.Number)
                .ToList();

            var departureTimes = await _getDepartureTimeRepository.GetAllDepartureTimes(route.IdTravelRoute);
            int turnosTotalesPorDia = departureTimes.Count;

            var vehiculosParaManana = todosLosVehiculos.Skip(turnosTotalesPorDia).ToList();

            var salidasProgramadas = new List<DepartureTimeByQueueDto>();
            DateTime tomorrow = DateTime.Today.AddDays(1);

            for (int i = 0; i < turnosTotalesPorDia; i++)
            {
                var horario = departureTimes[i];
                var vehiculo = i < vehiculosParaManana.Count ? vehiculosParaManana[i] : null;

                salidasProgramadas.Add(new DepartureTimeByQueueDto
                {
                    Id = horario.Id,
                    DepartureHour = tomorrow.Add(horario.Hour.ToTimeSpan()),
                    Vehicle = vehiculo != null ? new VehicleDto
                    {
                        Id = vehiculo.IdVehicle,
                        Photo = vehiculo.Photo != null ? Convert.ToBase64String(vehiculo.Photo) : null,
                        AvailableSeats = vehiculo.SeatVehicles.Count(sv => sv.StateSeat == true)
                    } : null
                });
            }

            return new DataRouteDto
            {
                IdRoute = route.IdTravelRoute,
                NameRoute = route.NameRoute,
                Price = (double)route.Price,
                DepartureHour = salidasProgramadas
            };
        }

    }
}
