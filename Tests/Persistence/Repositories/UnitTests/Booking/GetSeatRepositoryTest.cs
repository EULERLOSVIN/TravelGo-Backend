using Application.Interfaces.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence;
using Persistence.Context;
using Persistence.Repositories.Booking;

namespace Tests.Persistence.Repositories.UnitTests.Booking
{
    public class GetSeatRepositoryTest
    {
       private ApplicationDbContext GetInMemoryContext()
       {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetSeatByIdOfVehicle_WhenSeatsExist_ShouldReturnMappedDtoList()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            
            // Creamos datos relacionados para satisfacer la navegación del EF
            var seat = new Seat { IdSeat = 1, Number = 3 };
            context.Seats.Add(seat);

            context.SeatVehicles.Add(new SeatVehicle
            {
                IdSeatVehicle = 10,
                IdVehicle = 5,
                IdSeat = 1,
                StateSeat = true
            });

            await context.SaveChangesAsync();

            // Usamos la interfaz como tipo, implementada por la clase concreta
            IGetSeatRepository repository = new GetSeatRepository(context);

            // ACT
            var result = await repository.GetSeatByIdOfVehicle(5);

            // ASSERT
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(10, result[0].Id);
        }

        [Fact]
        public async Task GetSeatByIdOfVehicle_WhenVehicleDoesNotExist_ShouldReturnEmptyList()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            IGetSeatRepository repository = new GetSeatRepository(context);

            // ACT
            var result = await repository.GetSeatByIdOfVehicle(999);

            // ASSERT
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
