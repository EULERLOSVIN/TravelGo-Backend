//using Application.Common;
//using Application.DTOs;
//using Application.Interfaces.ManagementUser;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using Moq;
//using Persistence;
//using Persistence.Context;
//using Persistence.Repositories;

//namespace Tests.Persistence.Repositories.UnitTests.Authentication
//{
//    public class RegisterUserRepositoryTests
//    {
//        private ApplicationDbContext GetInMemoryContext()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
//                .Options;

//            return new ApplicationDbContext(options);
//        }
//        #region RegisterUserRepository
//        [Fact]
//        public async Task RegisterUserRepository_whenInputDataIsValid_shouldReturnTrue()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();
//            var generateUniqueEmailMock = new Mock<IGenerateUniqueEmailRepository>();
//            generateUniqueEmailMock
//                 .Setup(x => x.GenerateUniqueEmail(It.IsAny<string>(), It.IsAny<string>()))
//                 .ReturnsAsync("euler.martinez@travelgo.com");

//            // Inyectamos el .Object del mock
//            var instance = new RegisterUserRepository(context, generateUniqueEmailMock.Object);

//            var inputData = new RegisterUserDto
//            {
//                firstName = "Euler",
//                lastName = "Martinez Hurtado",
//                idRole = 1,
//                typeDocument = 1,
//                numberIdentityDocument = "12345678",
//                phoneNumber = "999000111",
//                password = "test123"
//            };

//            // ACT
//            bool resultObtained = await instance.RegisterUser(inputData);          
//            // ASSERT
//            Assert.True(resultObtained);
//        }

//        [Fact]
//        public async Task RegisterUserRepository_whenInputDataIsNull_shouldReturnFalse()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();
//            var emailMock = new Mock<IGenerateUniqueEmailRepository>();
//            var instance = new RegisterUserRepository(context, emailMock.Object);

//            var inputData = new RegisterUserDto
//            {
//                firstName = "",
//                lastName = "",
//                idRole = 0,
//                typeDocument = 0,
//                numberIdentityDocument = "",
//                phoneNumber = "",
//                password = ""
//            };

//            // ACT
//            bool resultObtaind = await instance.RegisterUser(inputData);

//            // ASSERT
//            Assert.False(resultObtaind);
//        }

//        [Fact]
//        public async Task RegisterUserRepository_WhenDocumentOfIdentityAlreadyExists_ShouldReturnFalse()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();
//            var emailMock = new Mock<IGenerateUniqueEmailRepository>();
//            var instance = new RegisterUserRepository(context, emailMock.Object);

//            // Insertamos un usuario previo con el mismo documento
//            context.People.Add(new Person 
//            { NumberIdentityDocument = "12345678", 
//                PhoneNumber = "999000111", 
//                FirstName = "X", 
//                LastName = "Y", 
//                IdTypeDocument = 1 
//            });

//            await context.SaveChangesAsync();

//            var inputData = new RegisterUserDto
//            {
//                firstName = "Euler",
//                lastName = "Martinez",
//                numberIdentityDocument = "12345678", // DUPLICADO
//                phoneNumber = "900000000",
//                password = "password123",
//                idRole = 1,
//                typeDocument = 1
//            };

//            // ACT
//            bool result = await instance.RegisterUser(inputData);

//            // ASSERT
//            Assert.False(result);
//        }

//        [Fact]
//        public async Task RegisterUser_WhenPhoneNumberAlreadyExists_ShouldReturnFalse()
//        {
//            // ARRANGE
//            using var context = GetInMemoryContext();
//            var emailMock = new Mock<IGenerateUniqueEmailRepository>();
//            var instance = new RegisterUserRepository(context, emailMock.Object);

//            // Insertamos un usuario previo con el mismo documento
//            context.People.Add(new Person
//            {
//                NumberIdentityDocument = "12345678",
//                PhoneNumber = "999000111",
//                FirstName = "X",
//                LastName = "Y",
//                IdTypeDocument = 1
//            });

//            await context.SaveChangesAsync();

//            var inputData = new RegisterUserDto
//            {
//                firstName = "Euler",
//                lastName = "Martinez",
//                numberIdentityDocument = "45623745",
//                phoneNumber = "999000111",
//                password = "password123",
//                idRole = 1,
//                typeDocument = 1
//            };

//            // ACT
//            bool result = await instance.RegisterUser(inputData);

//            // ASSERT
//            Assert.False(result);
//        }
//        #endregion
//    }
//}