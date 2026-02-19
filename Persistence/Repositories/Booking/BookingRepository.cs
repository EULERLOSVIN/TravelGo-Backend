using Application.Interfaces.Booking;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Threading;

namespace Persistence.Repositories.Booking
{
    public class BookingRepository: IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SelectSeat(int idSeatVehicle)
        {
            // Esperamos nuestro turno para entrar
            await _semaphore.WaitAsync();

            try
            {
                // Ahora que estamos solos, buscamos el asiento
                var seat = await _context.SeatVehicles
                    .FirstOrDefaultAsync(s => s.IdSeatVehicle == idSeatVehicle && s.IdStateSeatVehicle == 0);

                if (seat == null) return false;

                seat.IdStateSeatVehicle = 1; // Reservado

                _context.SeatVehicles.Update(seat);
                return await _context.SaveChangesAsync() > 0;
            }
            finally
            {
                // Liberamos el semáforo para el siguiente hilo
                _semaphore.Release();
            }
        }

        // REGISTRAR DATOS DEL PASAJERO
        public async Task<bool> UpdatePassengerDetails(int idSeatVehicle, string dni, string fullName, string pickUp)
        {
            var seat = await _context.SeatVehicles.FirstOrDefaultAsync(s => s.IdSeatVehicle == idSeatVehicle);

            if (seat != null && seat.IdStateSeatVehicle == 1)
            {
                _context.SeatVehicles.Update(seat);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        // CONFIRMAR PAGO
        public async Task<bool> ConfirmPayment(int idSeatVehicle)
        {
            var seat = await _context.SeatVehicles.FirstOrDefaultAsync(s => s.IdSeatVehicle == idSeatVehicle);
            if (seat == null) return false;

            seat.IdStateSeatVehicle = 2;
            _context.SeatVehicles.Update(seat);
            return await _context.SaveChangesAsync() > 0;
        }

        // LIBERAR ASIENTO (Corrección)
        public async Task<bool> ReleaseSeat(int idSeatVehicle)
        {
            var seat = await _context.SeatVehicles.FirstOrDefaultAsync(s => s.IdSeatVehicle == idSeatVehicle);
            if (seat == null) return false;

            seat.IdStateSeatVehicle = 0;
            _context.SeatVehicles.Update(seat);
            return await _context.SaveChangesAsync() > 0;
        }

        // CRONÓMETRO DE 10 MINUTOS
        public async Task ReleaseSeatIfPending(int idSeatVehicle)
        {
            var seat = await _context.SeatVehicles.FindAsync(idSeatVehicle);
            if (seat != null && seat.IdStateSeatVehicle == 1)
            {
                seat.IdStateSeatVehicle = 0;
                await _context.SaveChangesAsync();
            }
        }
    }
}