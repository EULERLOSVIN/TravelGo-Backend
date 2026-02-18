using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.ManagementUser;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenerateUniqueEmailRepository _generateUniqueEmailRepository;

        public RegisterUserRepository(ApplicationDbContext context, IGenerateUniqueEmailRepository generateUniqueEmailRepository)
        {
            _context = context;
            _generateUniqueEmailRepository = generateUniqueEmailRepository;
        }

        public async Task<bool> RegisterUser(RegisterUserDto dto)
        {           
            if (dto == null || string.IsNullOrEmpty(dto.password)) return false;

            bool alreadyExists = await _context.People.AnyAsync(p =>
            p.NumberIdentityDocument == dto.numberIdentityDocument ||
            p.PhoneNumber == dto.phoneNumber);

            if (alreadyExists) return false;
            if(dto.typeDocument == 1 && dto.numberIdentityDocument.Length != 8) return false;

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

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}