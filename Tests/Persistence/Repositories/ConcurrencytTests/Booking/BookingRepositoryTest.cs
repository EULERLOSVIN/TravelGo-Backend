//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using Persistence;
//using Persistence.Context;
//using Persistence.Repositories.Booking;
//using Xunit;

//namespace Tests.Persistence.Repositories.ConcurrencyTests.Booking
//{
//    public class TestDbContext : ApplicationDbContext
//    {
//        public TestDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//            modelBuilder.Entity<SeatVehicle>().HasKey(s => s.IdSeatVehicle);
//        }
//    }

//    public class BookingRepositoryTests
//    {
//        private DbContextOptions<ApplicationDbContext> GetInMemoryOptions()
//        {
//            return new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
//                .Options;
//        }

//        [Fact]
//        public async Task SelectSeat_CuandoVariosUsuariosCompitenPorElMismoAsiento_SoloUnoDebeGanar()
//        {
//            //---ARRANGE
//            var options = GetInMemoryOptions();
//            int asientoIdCompartido = 10;
//            int totalUsuariosSimultaneos = 10000;

//            using (var contextSetup = new TestDbContext(options))
//            {
//                contextSetup.SeatVehicles.Add(new SeatVehicle { IdSeatVehicle = asientoIdCompartido, StateSeat = true });
//                await contextSetup.SaveChangesAsync();
//            }

//            //---ACT
//            var listaDePeticiones = new List<Task<bool>>();
//            for (int i = 0; i < totalUsuariosSimultaneos; i++)
//            {
//                listaDePeticiones.Add(Task.Run(async () =>
//                {
//                    using var context = new TestDbContext(options);
//                    var repo = new BookingRepository(context);
//                    return await repo.SelectSeat(asientoIdCompartido);
//                }));
//            }

//            var resultados = await Task.WhenAll(listaDePeticiones);

//            //---ASSERT
//            int exitos = resultados.Count(r => r == true);
//            int fallos = resultados.Count(r => r == false);

//            Assert.Equal(1, exitos);
//            Assert.Equal(totalUsuariosSimultaneos - 1, fallos);
//        }

//        [Fact]
//        public async Task SelectSeat_CuandoDiferentesUsuariosReservanDiferentesAsientos_TodosDebenGanar()
//        {
//            //---ARRANGE
//            var options = GetInMemoryOptions();
//            int cantidadAsientos = 1000;

//            using (var contextSetup = new TestDbContext(options))
//            {
//                for (int i = 1; i <= cantidadAsientos; i++)
//                    contextSetup.SeatVehicles.Add(new SeatVehicle { IdSeatVehicle = i, StateSeat = false });
//                await contextSetup.SaveChangesAsync();
//            }

//            //---ACT
//            var listaDePeticiones = new List<Task<bool>>();
//            for (int i = 1; i <= cantidadAsientos; i++)
//            {
//                int idActual = i;
//                listaDePeticiones.Add(Task.Run(async () =>
//                {
//                    using var context = new TestDbContext(options);
//                    var repo = new BookingRepository(context);
//                    return await repo.SelectSeat(idActual);
//                }));
//            }

//            var resultados = await Task.WhenAll(listaDePeticiones);
//            int totalExitosos = resultados.Count(r => r == true);

//            //---ASSERT
//            Assert.Equal(cantidadAsientos, totalExitosos);
//        }
//    }
//}