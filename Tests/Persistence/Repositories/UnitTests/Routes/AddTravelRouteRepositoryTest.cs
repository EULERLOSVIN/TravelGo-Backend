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
        // 1. HELPER: Configura una base de datos en memoria fresca para cada test.
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new ApplicationDbContext(options);
        }

        // ======================================================================================
        // PRUEBA 1: Creación Exitosa
        // Objetivo: Verificar que la ruta se guarde correctamente cuando los datos son válidos.
        // ======================================================================================
        [Fact]
        public async Task AddTravelRoute_WhenDataIsValid_ShouldReturnId()
        {
            // 1. ARRANGE
            using var context = GetInMemoryContext();
            var repository = new AddTravelRouteRepository(context);

            // Insertamos Lugares (Places) necesarios para conectar la ruta
            // Recordar: No podemos crear una ruta entre lugares que no existen.
            var placeA = new Place { Name = "Lima", Description = "Capital" };
            var placeB = new Place { Name = "Cusco", Description = "Ombligo del mundo" };
            
            context.Places.Add(placeA);
            context.Places.Add(placeB);
            await context.SaveChangesAsync();

            // Datos de la nueva ruta
            var inputData = new AddTravelRouteDto
            {
                idPlaceA = placeA.IdPlace, // Usamos los IDs generados
                idPlaceB = placeB.IdPlace,
                price = 150.50m,
                isActive = true
            };

            // 2. ACT
            int resultId = await repository.AddTravelRoute(inputData);

            // 3. ASSERT
            // Verificamos que devuelva un ID válido (mayor a 0)
            Assert.True(resultId > 0);

            // Verificamos en la BD falsa
            var savedRoute = await context.TravelRoutes.FindAsync(resultId);
            Assert.NotNull(savedRoute);
            Assert.Equal("Lima - Cusco", savedRoute.NameRoute); // El nombre se genera automáticamente
        }

        // ======================================================================================
        // PRUEBA 2: Origen igual a Destino
        // Objetivo: Verificar que falle si intentamos crear una ruta circular (A -> A).
        // ======================================================================================
        [Fact]
        public async Task AddTravelRoute_WhenOriginEqualsDestination_ShouldThrowException()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            var repository = new AddTravelRouteRepository(context);

            var inputData = new AddTravelRouteDto
            {
                idPlaceA = 1,
                idPlaceB = 1, // <--- ERROR: Origen = Destino
                price = 100,
                isActive = true
            };

            // ACT & ASSERT
            // Aquí usamos Assert.ThrowsAsync porque el repositorio Lanza una Excepción, no devuelve false.
            var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddTravelRoute(inputData));
            
            // Opcional: Verificar el mensaje de error
            Assert.Equal("El origen y el destino no pueden ser el mismo lugar.", exception.Message);
        }

        // ======================================================================================
        // PRUEBA 3: Lugares Inexistentes
        // Objetivo: Verificar que falle si intentamos usar un ID de lugar que no existe en BD.
        // ======================================================================================
        [Fact]
        public async Task AddTravelRoute_WhenPlaceDoesNotExist_ShouldThrowException()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            var repository = new AddTravelRouteRepository(context);

            // No insertamos ningun "Place" en la BD, así que está vacía.

            var inputData = new AddTravelRouteDto
            {
                idPlaceA = 999, // No existe
                idPlaceB = 888, // No existe
                price = 100
            };

            // ACT & ASSERT
            var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddTravelRoute(inputData));
            
            // Verificamos que el mensaje sea sobre que el lugar no existe
            Assert.Contains("no existe", exception.Message);
        }
    }
}
