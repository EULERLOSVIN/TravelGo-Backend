using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Repositories
{
    public class EditUserRepository: IEditUserRepository
    {
        public readonly ApplicationDbContext _context;
        private readonly GenerateUniqueEmailRepository _generateUniqueEmailRepository;

        public EditUserRepository(ApplicationDbContext context , GenerateUniqueEmailRepository generateUniqueEmailRepository)
        {
            _context = context;
            _generateUniqueEmailRepository = generateUniqueEmailRepository;
        }

        public async Task<bool> EditUser(EditPersonnelDto newData)
        {
            var account = await _context.Accounts
                .Include(a => a.IdPersonNavigation)
                .FirstOrDefaultAsync(a => a.IdAccount == newData.IdAccount);

            if (account == null)
            {
                throw new KeyNotFoundException($"No se encontró la cuenta con ID {newData.IdAccount}");
            }

            var person = account.IdPersonNavigation;
            if (person.FirstName != newData.FirstName || person.LastName != newData.LastName)
            {
                account.Email = await _generateUniqueEmailRepository.GenerateUniqueEmail(newData.FirstName, newData.LastName);
            }

            person.FirstName = newData.FirstName;
            person.LastName = newData.LastName;
            person.IdTypeDocument = newData.IdTypeDocument;
            person.NumberIdentityDocument = newData.NumberDocument;
            person.PhoneNumber = newData.PhoneNumber;


            account.IdRole = newData.IdRole;
            account.IdStateAccount = newData.IdStateOfAccount;

            if (!string.IsNullOrWhiteSpace(newData.NewPassword))
            {
                if (newData.NewPassword != newData.ConfirmPassword)
                {
                    throw new ArgumentException("La nueva contraseña y la confirmación no coinciden.");
                }

                account.Password = BCrypt.Net.BCrypt.HashPassword(newData.NewPassword);
            }

            var result = await _context.SaveChangesAsync();
            return true;
        }
    }
}
