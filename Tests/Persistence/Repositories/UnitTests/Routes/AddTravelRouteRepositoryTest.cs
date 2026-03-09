using Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence;
using Persistence.Context;
using Persistence.Repositories;

namespace Tests.Persistence.Repositories.UnitTests.Routes
{
    public class AddTravelRouteRepositoryTest
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
        public async Task AddTravelRoute_WhenDataIsValid_ShouldReturnId()
        {
            // 1. ARRANGE
            using var context = GetInMemoryContext();
            var repository = new AddTravelRouteRepository(context);

            var placeA = new Place { Name = "Lima", Description = "Capital" };
            var placeB = new Place { Name = "Cusco", Description = "Ombligo del mundo" };

            context.Places.Add(placeA);
            context.Places.Add(placeB);
            await context.SaveChangesAsync();

            var inputData = new AddTravelRouteDto
            {
                idPlaceA = placeA.IdPlace,
                idPlaceB = placeB.IdPlace,
                price = 150.50m,
                isActive = true
            };

            // 2. ACT
            int resultId = await repository.AddTravelRoute(inputData);

            // 3. ASSERT
            Assert.True(resultId > 0);

            var savedRoute = await context.TravelRoutes.FindAsync(resultId);
            Assert.NotNull(savedRoute);
            Assert.Equal("Lima - Cusco", savedRoute.NameRoute);
        }

        [Fact]
        public async Task AddTravelRoute_WhenOriginEqualsDestination_ShouldThrowException()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            var repository = new AddTravelRouteRepository(context);

            var inputData = new AddTravelRouteDto
            {
                idPlaceA = 1,
                idPlaceB = 1,
                price = 100,
                isActive = true
            };

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddTravelRoute(inputData));

            Assert.Equal("El origen y el destino no pueden ser el mismo lugar.", exception.Message);
        }

        [Fact]
        public async Task AddTravelRoute_WhenPlaceDoesNotExist_ShouldThrowException()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            var repository = new AddTravelRouteRepository(context);

            var inputData = new AddTravelRouteDto
            {
                idPlaceA = 999, // No existe
                idPlaceB = 888, // No existe
                price = 100
            };

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddTravelRoute(inputData));
            Assert.Contains("no existe", exception.Message);
        }
    }
}
