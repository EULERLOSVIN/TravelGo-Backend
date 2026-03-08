using Domain.Entities;
﻿
using Application.DTOs.Headquarters;
using Application.Interfaces.Booking;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Booking
{
    public class GetSeatRepository: IGetSeatRepository
    {
        public readonly ApplicationDbContext _context;
        public GetSeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SeatDto>> GetSeatByIdOfVehicle(int idSeatVehicle)
        {
            var seats = await _context.SeatVehicles.Where(s => s.IdVehicle == idSeatVehicle)
                .Select(s => new SeatDto
                {
                    Id = s.IdSeatVehicle,
                    Number = s.IdSeatNavigation.Number,
                    IsAvailable = s.StateSeat
                }).ToListAsync();
            if (seats == null)
            {
                return null!;
            }

            return seats;
        }
    }
}
