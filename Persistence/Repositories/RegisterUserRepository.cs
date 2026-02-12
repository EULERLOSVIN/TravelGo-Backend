using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Globalization;
using System.Text;

namespace Persistence.Repositories
{
    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly GenerateUniqueEmailRepository _generateUniqueEmailRepository;

        public RegisterUserRepository(ApplicationDbContext context, GenerateUniqueEmailRepository generateUniqueEmailRepository)
        {
            _context = context;
            _generateUniqueEmailRepository = generateUniqueEmailRepository;
        }

        public async Task<int> RegisterUser(RegisterUserDto dto)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var person = new Person
                {
                    FirstName = dto.firstName,
                    LastName = dto.lastName,
                    PhoneNumber = dto.phoneNumber,
                    IdTypeDocument = dto.typeDocument,
                    NumberIdentityDocument = dto.numberIdentityDocument
                };

                _context.People.Add(person);
                await _context.SaveChangesAsync();

                string generatedEmail = await _generateUniqueEmailRepository.GenerateUniqueEmail(dto.firstName, dto.lastName);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.password);
                var account = new Account
                {
                    IdPerson = person.IdPerson,
                    IdRole = dto.idRole,
                    Email = generatedEmail,
                    Password = passwordHash
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return account.IdAccount;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}