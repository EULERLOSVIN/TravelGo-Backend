using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence;
using Persistence.Context;
using Persistence.Repositories.Booking;

namespace Tests.Persistence.Repositories.UnitTests.Booking
{
    public class GetDepartureTimeRepositoryTest
    {
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            return new ApplicationDbContext(options);
        }

        private void Seed(ApplicationDbContext context)
        {
            // Datos para probar filtros de tiempo
            context.DepartureTimes.AddRange(new List<DepartureTime>
            {
                new DepartureTime { IdDepartureTime = 1, IdTravelRoute = 1, Hour = new TimeOnly(08, 00) },
                new DepartureTime { IdDepartureTime = 2, IdTravelRoute = 1, Hour = new TimeOnly(14, 00) },
                new DepartureTime { IdDepartureTime = 3, IdTravelRoute = 1, Hour = new TimeOnly(20, 00) }
            });
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllDepartureTimes_ShouldReturnAllTimes()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            Seed(context);
            var repository = new GetDepartureTimeRepository(context);

            // ACT
            var result = await repository.GetAllDepartureTimes(1);

            // ASSERT
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetRemainingDepartureTimes_ShouldFilterOnlyFutureTimes()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            Seed(context);
            var repository = new GetDepartureTimeRepository(context);

            // ACT
            var result = await repository.GetRemainingDepartureTimes(1);

            // ASSERT
            Assert.NotNull(result);
            foreach (var item in result)
            {
                Assert.True(item.Hour >= TimeOnly.FromDateTime(DateTime.Now));
            }
        }
    }
}
