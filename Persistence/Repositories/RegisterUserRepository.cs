using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class RegisterUserRepository: IRegisterUserRepository
    {
        private readonly ApplicationDbContext _context;

        public RegisterUserRepository(ApplicationDbContext context)
        {
            _context = context;
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

                string generatedEmail = await GenerateUniqueEmail(dto.firstName, dto.lastName);
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

        private async Task<string> GenerateUniqueEmail(string firstName, string lastName)
        {
            string firstPart = firstName.Trim().Split(' ')[0].ToLower();
            string lastPart = lastName.Trim().Split(' ')[0].ToLower();

            string baseEmail = $"{firstPart}.{lastPart}";
            string domain = "@travelgo.com";
            string finalEmail = baseEmail + domain;

            int counter = 1;

            while (await _context.Accounts.AnyAsync(a => a.Email == finalEmail))
            {
                finalEmail = $"{baseEmail}{counter}{domain}";
                counter++;
            }

            return finalEmail;
        }


    }
}