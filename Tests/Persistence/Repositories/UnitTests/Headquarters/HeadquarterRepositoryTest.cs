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
        // Configura una base de datos en memoria fresca para cada test.
        
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nombre único = BD nueva
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new ApplicationDbContext(options);
        }

        #region CreateAsync Tests (Pruebas de Creación de Sede)

        // ======================================================================================
        // PRUEBA 1: Registro Exitoso
        // ======================================================================================
        [Fact]
        public async Task CreateAsync_WhenInputDataIsValid_ShouldReturnTrue()
        {
            // BD falsa
            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);

            
            var inputData = new CreateHeadquarterDto
            {
                IdCompany = 1,
                IdStateHeadquarter = 1, // 1 = Activa
                Name = "Sede Norte",
                Address = "Av. Principal 123",
                Department = "Lima",
                Province = "Lima",
                District = "Los Olivos",
                Phone = "999888777",
                Email = "norte@travelgo.com",
                IsMain = false
            };

            
            // al método CreateAsync del repositorio
            bool result = await repository.CreateAsync(inputData);
           
            // 1 debe devolver TRUE (éxito)
            Assert.True(result); 
            
            // consultamos la BD falsa para ver si realmenté se guardó
            var savedHeadquarter = await context.Headquarters.FirstOrDefaultAsync(h => h.Name == "Sede Norte");
            Assert.NotNull(savedHeadquarter); // No debe ser nulo (debe existir)
            Assert.Equal("Sede Norte", savedHeadquarter.Name); // El nombre guardado debe ser correcto
        }

        // ======================================================================================
        // PRUEBA 2: Datos Nulos      
        // ======================================================================================
        [Fact]
        public async Task CreateAsync_WhenDtoIsNull_ShouldReturnFalse()
        {
            // ARRANGE
            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);

            CreateHeadquarterDto inputData = null; // Simulamos el error enviando null
         
            // Llamamos al método con el dato nulo
            bool result = await repository.CreateAsync(inputData);

            
            // Esperamos FALSE
            Assert.False(result); 
        }

        // ======================================================================================
        // PRUEBA 3: Casillas Vacías (Nombre Obligatorio)      
        // ======================================================================================
        [Fact]
        public async Task CreateAsync_WhenNameIsEmpty_ShouldReturnFalse()
        {
           
            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);

            var inputData = new CreateHeadquarterDto
            {
                IdCompany = 1,
                IdStateHeadquarter = 1,
                Name = "", // <--- ERROR: Nombre vacío ("")
                Address = "Calle Falsa 123",
                Department = "Lima",
                Province = "Lima",
                District = "Lima",
                Phone = "123456",
                IsMain = false
            };

           
            bool result = await repository.CreateAsync(inputData);

            
            // Esperamos FALSE
            Assert.False(result); 
        }

        // ======================================================================================
        // PRUEBA 4: Duplicados
        // ======================================================================================
        [Fact]
        public async Task CreateAsync_WhenNameAlreadyExists_ShouldReturnFalse()
        {
            
            using var context = GetInMemoryContext();
            var repository = new HeadquarterRepository(context);


            context.Headquarters.Add(new Headquarter
            {
                IdCompany = 1,
                IdStateHeadquarter = 1,
                Name = "Sede Central", // <--- Ya existe este nombre
                Address = "Av. Original",
                Department = "Lima",
                Province = "Lima",
                District = "Miraflores",
                Phone = "000000",
                IsMain = true,
                RegistrationDate = DateTime.Now
            });
            await context.SaveChangesAsync(); // Guardamos el estado inicial

            // crear OTRA con el MISMO nombre
            var duplicateData = new CreateHeadquarterDto
            {
                IdCompany = 1,
                IdStateHeadquarter = 1,
                Name = "Sede Central", // <--- INTENTO DE DUPLICADO
                Address = "Otra dirección",
                Department = "Cusco",
                Province = "Cusco",
                District = "Cusco",
                Phone = "999999",
                IsMain = false
            };

            
            bool result = await repository.CreateAsync(duplicateData);
            
            // Esperamos FALSE, porque el repositorio busca si ya existe el nombre
            Assert.False(result); 
        }

        #endregion
    }
}
