using Application.DTOs.Headquarters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence.Context;
using Persistence.Repositories.Headquarters;
using Persistence;

namespace Tests.Persistence.Repositories.UnitTests.Headquarters
{
    public class HeadquarterRepositoryTests
    {
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new ApplicationDbContext(options);
        }

        #region CreateAsync Tests (Pruebas de Creación de Sede)

        [Fact]
        public async Task CreateAsync_WhenInputDataIsValid_ShouldReturnTrue()
        {
            // BD falsa
            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);


            var inputData = new CreateHeadquarterDto
            {
                IdCompany = 1,
                IdStateHeadquarter = 1,
                Name = "Sede Norte",
                Address = "Av. Principal 123",
                Department = "Lima",
                Province = "Lima",
                District = "Los Olivos",
                Phone = "999888777",
                Email = "norte@travelgo.com",
                IsMain = false
            };


            bool result = await repository.CreateAsync(inputData);

            Assert.True(result);

            var savedHeadquarter = await context.Headquarters.FirstOrDefaultAsync(h => h.Name == "Sede Norte");
            Assert.NotNull(savedHeadquarter);
            Assert.Equal("Sede Norte", savedHeadquarter.Name);
        }

        [Fact]
        public async Task CreateAsync_WhenDtoIsNull_ShouldReturnFalse()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);

            CreateHeadquarterDto inputData = null;
            bool result = await repository.CreateAsync(inputData);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_WhenNameIsEmpty_ShouldReturnFalse()
        {

            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);

            var inputData = new CreateHeadquarterDto
            {
                IdCompany = 1,
                IdStateHeadquarter = 1,
                Name = "",
                Address = "Calle Falsa 123",
                Department = "Lima",
                Province = "Lima",
                District = "Lima",
                Phone = "123456",
                IsMain = false
            };


            bool result = await repository.CreateAsync(inputData);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsync_WhenNameAlreadyExists_ShouldReturnFalse()
        {

            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);


            context.Headquarters.Add(new Headquarter
            {
                IdCompany = 1,
                IdStateHeadquarter = 1,
                Name = "Sede Central",
                Address = "Av. Original",
                Department = "Lima",
                Province = "Lima",
                District = "Miraflores",
                Phone = "000000",
                IsMain = true,
                RegistrationDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            var duplicateData = new CreateHeadquarterDto
            {
                IdCompany = 1,
                IdStateHeadquarter = 1,
                Name = "Sede Central",
                Address = "Otra dirección",
                Department = "Cusco",
                Province = "Cusco",
                District = "Cusco",
                Phone = "999999",
                IsMain = false
            };


            bool result = await repository.CreateAsync(duplicateData);
            Assert.False(result);
        }

        #endregion
    }
}
