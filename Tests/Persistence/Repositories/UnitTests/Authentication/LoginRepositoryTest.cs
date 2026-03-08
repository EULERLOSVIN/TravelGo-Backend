//using Application.DTOs;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Persistence;
//using Persistence.Context;
//using Persistence.Repositories;
//using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
//namespace Tests.Persistence.Repositories.UnitTests.Authentication
//{
//    public class LoginRepositoryTests
//    {
//        private readonly Mock<IConfiguration> _configMock;

//        public LoginRepositoryTests()
//        {
//            _configMock = new Mock<IConfiguration>();

//            _configMock.Setup(x => x[It.IsAny<string>()]).Returns((string key) =>
//            {
//                return key switch
//                {
//                    "Jwt:Key" => "Esta_Es_Una_Llave_Super_Secreta_2026_TravelGo",
//                    "Jwt:Issuer" => "TravelGo",
//                    "Jwt:Audience" => "TravelGoUsers",
//                    _ => null
//                };
//            });
//        }

//        private ApplicationDbContext GetInMemoryContext()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;
//            return new ApplicationDbContext(options);
//        }

//        [Fact]
//        public async Task LoginRepository_WhenCredentialsAreValid_ShouldReturnLoginResponse()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();

//            // Es vital hashear el password para que BCrypt.Verify funcione
//            var passwordHashed = BCrypt.Net.BCrypt.HashPassword("Password123");

//            // Construimos la cuenta con todas sus relaciones cargadas
//            var account = new Account
//            {
//                Email = "euler.test@travelgo.com",
//                Password = passwordHashed,
//                IdRoleNavigation = new Role { Name = "Admin" },
//                IdStateAccountNavigation = new StateAccount { Name = "Activo" },
//                IdPersonNavigation = new Person
//                {
//                    FirstName = "Euler",
//                    LastName = "Martinez",
//                    NumberIdentityDocument = "70654321"
//                }
//            };

//            context.Accounts.Add(account);
//            await context.SaveChangesAsync();

//            // Usamos el _configMock que configuramos previamente para el JWT
//            var instance = new LoginRepository(context, _configMock.Object);
//            var loginDto = new LoginRequestDto { Email = "euler.test@travelgo.com", Password = "Password123" };

//            // ACT
//            var result = await instance.Login(loginDto);

//            // ASSERT
//            Assert.NotNull(result);
//            Assert.Equal("euler.test@travelgo.com", result.Email);
//            Assert.Equal("Admin", result.Rol);
//            Assert.False(string.IsNullOrEmpty(result.Token));
//        }

//        [Fact]
//        public async Task LoginRepository_WhenPasswordIsIncorrect_ShouldReturnNull()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();

//            // Es vital hashear el password para que BCrypt.Verify funcione
//            var passwordHashed = BCrypt.Net.BCrypt.HashPassword("test1234");

//            // Construimos la cuenta con todas sus relaciones cargadas
//            var account = new Account
//            {
//                Email = "euler.test@travelgo.com",
//                Password = passwordHashed,
//                IdRoleNavigation = new Role { Name = "Admin" },
//                IdPersonNavigation = new Person
//                {
//                    FirstName = "Euler",
//                    LastName = "Martinez",
//                    NumberIdentityDocument = "70654321"
//                }
//            };

//            context.Accounts.Add(account);
//            await context.SaveChangesAsync();

//            // Usamos el _configMock que configuramos previamente para el JWT
//            var instance = new LoginRepository(context, _configMock.Object);
//            var loginDto = new LoginRequestDto { Email = "euler.test@travelgo.com", Password = "Password123" };

//            // ACT
//            var result = await instance.Login(loginDto);

//            // ASSERT
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task LoginRepository_WhenCredentialsNull_ShouldReturnNull()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();

//            // Es vital hashear el password para que BCrypt.Verify funcione
//            var passwordHashed = BCrypt.Net.BCrypt.HashPassword("test1234");

//            // Construimos la cuenta con todas sus relaciones cargadas
//            var account = new Account
//            {
//                Email = "euler.test@travelgo.com",
//                Password = passwordHashed,
//                IdRoleNavigation = new Role { Name = "Admin" },
//                IdPersonNavigation = new Person
//                {
//                    FirstName = "Euler",
//                    LastName = "Martinez",
//                    NumberIdentityDocument = "70654321"
//                }
//            };

//            context.Accounts.Add(account);
//            await context.SaveChangesAsync();

//            // Usamos el _configMock que configuramos previamente para el JWT
//            var instance = new LoginRepository(context, _configMock.Object);
//            var loginDto = new LoginRequestDto { Email = "incorrect.test@travelgo.com", Password = "test1234" };

//            // ACT
//            var result = await instance.Login(loginDto);

//            // ASSERT
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task LoginRepository_WhenEmailIsIncorrect_ShouldReturnNull()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();

//            // Es vital hashear el password para que BCrypt.Verify funcione
//            var passwordHashed = BCrypt.Net.BCrypt.HashPassword("test1234");

//            // Construimos la cuenta con todas sus relaciones cargadas
//            var account = new Account
//            {
//                Email = "euler.test@travelgo.com",
//                Password = passwordHashed,
//                IdRoleNavigation = new Role { Name = "Admin" },
//                IdPersonNavigation = new Person
//                {
//                    FirstName = "Euler",
//                    LastName = "Martinez",
//                    NumberIdentityDocument = "70654321"
//                }
//            };

//            context.Accounts.Add(account);
//            await context.SaveChangesAsync();

//            // Usamos el _configMock que configuramos previamente para el JWT
//            var instance = new LoginRepository(context, _configMock.Object);
//            var loginDto = new LoginRequestDto { Email = "incorrect.test@travelgo.com", Password = "test1234" };

//            // ACT
//            var result = await instance.Login(loginDto);

//            // ASSERT
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task LoginRepository_WhenCedentialsIsNull_ShouldReturnNull()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();

//            // Es vital hashear el password para que BCrypt.Verify funcione
//            var passwordHashed = BCrypt.Net.BCrypt.HashPassword("test1234");

//            // Construimos la cuenta con todas sus relaciones cargadas
//            var account = new Account
//            {
//                Email = "euler.test@travelgo.com",
//                Password = passwordHashed,
//                IdRoleNavigation = new Role { Name = "Admin" },
//                IdPersonNavigation = new Person
//                {
//                    FirstName = "Euler",
//                    LastName = "Martinez",
//                    NumberIdentityDocument = "70654321"
//                }
//            };

//            context.Accounts.Add(account);
//            await context.SaveChangesAsync();

//            // Usamos el _configMock que configuramos previamente para el JWT
//            var instance = new LoginRepository(context, _configMock.Object);
//            var loginDto = new LoginRequestDto { Email = "", Password = "" };

//            // ACT
//            var result = await instance.Login(loginDto);

//            // ASSERT
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task LoginRepository_WhenCedentialsIsEmpty_ShouldReturnNull()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();

//            // Es vital hashear el password para que BCrypt.Verify funcione
//            var passwordHashed = BCrypt.Net.BCrypt.HashPassword("test1234");

//            // Construimos la cuenta con todas sus relaciones cargadas
//            var account = new Account
//            {
//                Email = "euler.test@travelgo.com",
//                Password = passwordHashed,
//                IdRoleNavigation = new Role { Name = "Admin" },
//                IdPersonNavigation = new Person
//                {
//                    FirstName = "Euler",
//                    LastName = "Martinez",
//                    NumberIdentityDocument = "70654321"
//                }
//            };

//            context.Accounts.Add(account);
//            await context.SaveChangesAsync();

            
//            var instance = new LoginRepository(context, _configMock.Object);
//            var loginDto = new LoginRequestDto { Email = " ", Password = " " };

          
//            var result = await instance.Login(loginDto);

//            // ASSERT
//            Assert.Null(result);
//        }
//    }
//}
