using Application.DTOs.Customers;
using Application.Interfaces.Booking;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Globalization;

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
            await _semaphore.WaitAsync();

            try
            {
                var seat = await _context.SeatVehicles
                    .FirstOrDefaultAsync(s => s.IdSeatVehicle == idSeatVehicle && s.StateSeat == true);

                if (seat == null) return false;
                seat.StateSeat = true;
                _context.SeatVehicles.Update(seat);
                return await _context.SaveChangesAsync() > 0;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // REGISTRAR DATOS DEL PASAJERO
        public async Task<bool> UpdatePassengerDetails(int idSeatVehicle, string dni, string fullName, string pickUp)
        {
            var seat = await _context.SeatVehicles.FirstOrDefaultAsync(s => s.IdSeatVehicle == idSeatVehicle);

            if (seat != null && seat.StateSeat == true)
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

            seat.StateSeat = false;
            _context.SeatVehicles.Update(seat);
            return await _context.SaveChangesAsync() > 0;
        }

        // LIBERAR ASIENTO
        public async Task<bool> ReleaseSeat(int idSeatVehicle)
        {
            var seat = await _context.SeatVehicles.FirstOrDefaultAsync(s => s.IdSeatVehicle == idSeatVehicle);
            if (seat == null) return false;

            seat.StateSeat = false;
            _context.SeatVehicles.Update(seat);
            return await _context.SaveChangesAsync() > 0;
        }

        // CRONÓMETRO DE 10 MINUTOS
        public async Task ReleaseSeatIfPending(int idSeatVehicle)
        {
            var seat = await _context.SeatVehicles.FindAsync(idSeatVehicle);
            if (seat != null && seat.StateSeat == true)
            {
                seat.StateSeat = false;
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> RegisterBooking(RegisterBookingDto dto)
        {
            await ValidatePaymentData(dto);        
            var (names, lastNames) = PartitionFullName(dto.FullName);
            int idMethodPayment = dto.OperationCode != null ? 1 : 2;

            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var person = new Person
                {
                    FirstName = names,
                    LastName = lastNames,
                    PhoneNumber = dto.PhoneNumeber,
                    IdTypeDocument = 1,
                    NumberIdentityDocument = dto.Dni
                };
                await _context.People.AddAsync(person);
                await _context.SaveChangesAsync();

                string serie = "F001";
                var lastBilling = await _context.Billings
                    .OrderByDescending(b => b.IdBilling)
                    .FirstOrDefaultAsync();
                int nextCorrelative = (lastBilling != null) ? lastBilling.IdBilling + 1 : 1;
                string documentNumber = $"{serie}-{nextCorrelative:D6}";

                var billing = new Billing
                {
                    IdPerson = person.IdPerson,
                    IdCompany = 1,
                    IdPaymentMethod = idMethodPayment,
                    BillingDate = DateTime.Now,
                    TotalAmount = dto.FullPayment,
                    DocumentNumber = documentNumber,
                    OperationPayCode = dto.OperationCode ?? dto.CardNumber
                };
                await _context.Billings.AddAsync(billing);
                await _context.SaveChangesAsync();

                var seatsSelected = await _context.SeatVehicles
                    .Include(sv => sv.IdSeatNavigation)
                    .Where(sv => dto.Seats.Contains(sv.IdSeatNavigation.Number) && sv.IdVehicle == dto.IdVehicle)
                    .ToListAsync();

                if (seatsSelected.Count != dto.Seats.Length)
                    throw new Exception("Algunos asientos seleccionados no están disponibles.");

                foreach (var seat in seatsSelected)
                {
                    // 1. Primero, aseguramos el estado del asiento
                    await SelectSeat(seat.IdSeatVehicle);

                    // 2. Generamos el código ANTES de crear el objeto
                    // (Esto evita que el objeto se cree con nulos si algo fallara en la generación)
                    string generatedCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                    // 3. Creamos el objeto
                    var ticket = new TravelTicket
                    {
                        IdBilling = billing.IdBilling,
                        IdTravelRoute = dto.IdRoute,
                        IdTicketState = 1,
                        IdVehicle = dto.IdVehicle,
                        SeatNumber = seat.IdSeatNavigation.Number,
                        TravelDate = dto.TravelDate, // Asegúrate de que esto sea DateTime o DateOnly
                        TicketCode = generatedCode
                    };

                    // 4. Lo añadimos
                    await _context.TravelTickets.AddAsync(ticket);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public Task ValidatePaymentData(RegisterBookingDto dto)
        {
            // Validación de DNI (8 números exactos)
            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Dni, @"^\d{8}$"))
                throw new Exception("El DNI debe tener 8 dígitos numéricos.");

            // Validación de Teléfono (9 números, empieza con 9)
            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.PhoneNumeber, @"^9\d{8}$"))
                throw new Exception("El teléfono debe tener 9 dígitos y empezar con 9.");

            // Validación de Correo (Opcional, pero debe ser @gmail.com)
            if (!string.IsNullOrEmpty(dto.Email) && !dto.Email.EndsWith("@gmail.com"))
                throw new Exception("Si proporciona un correo, debe ser de dominio @gmail.com.");

            // Validación condicional del método de pago
            bool isYape = !string.IsNullOrEmpty(dto.OperationCode);
            bool isCard = !string.IsNullOrEmpty(dto.CardNumber);

            if (isYape)
            {
                if (dto.OperationCode.Length < 6)
                    throw new Exception("El código de operación de Yape no es válido.");
            }
            else if (isCard)
            {
                if (dto.CardNumber.Length != 16)
                    throw new Exception("El número de tarjeta debe tener 16 dígitos.");
                if (string.IsNullOrEmpty(dto.ExpirationDate) || string.IsNullOrEmpty(dto.Cvv))
                    throw new Exception("Faltan datos de expiración o CVV de la tarjeta.");
            }
            else
            {
                throw new Exception("Debe proporcionar un método de pago válido (Yape o Tarjeta).");
            }

            return Task.CompletedTask;
        }

        public (string Names, string LastNames) PartitionFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return ("", "");

            string[] words = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i].ToLower());
            }

            if (words.Length >= 3)
            {
                string names = string.Join(" ", words.Take(words.Length - 2));
                string lastNames = string.Join(" ", words.Skip(words.Length - 2));

                return (names, lastNames);
            }

            if (words.Length == 2)
            {
                return (words[0], words[1]);
            }

            return (words[0], "");
        }
    }
}