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
            // 1. Buscar la cuenta incluyendo la navegación a la persona
            var account = await _context.Accounts
                .Include(a => a.IdPersonNavigation)
                .FirstOrDefaultAsync(a => a.IdAccount == newData.IdAccount);

            if (account == null)
            {
                throw new KeyNotFoundException($"No se encontró la cuenta con ID {newData.IdAccount}");
            }

            // 2. Actualizar Datos Personales (Tabla Person)
            var person = account.IdPersonNavigation;
            person.FirstName = newData.FirstName;
            person.LastName = newData.LastName;
            person.IdTypeDocument = newData.IdTypeDocument;
            person.NumberIdentityDocument = newData.NumberDocument;
            person.PhoneNumber = newData.PhoneNumber;

            // 3. Actualizar Datos de Cuenta (Tabla Account)
            account.IdRole = newData.IdRole;
            account.IdStateAccount = newData.IdStateOfAccount;
            account.Email = await _generateUniqueEmailRepository.GenerateUniqueEmail(newData.FirstName, newData.LastName);

            // Validación: Solo cambiar contraseña si ambos campos tienen datos y coinciden
            if (!string.IsNullOrEmpty(newData.NewPassword) && !string.IsNullOrEmpty(newData.ConfirmPassword))
            {
                if (newData.NewPassword == newData.ConfirmPassword)
                {
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(newData.NewPassword);
                    account.Password = passwordHash;
                }
                else
                {
                    // Opcional: Lanzar una excepción si las contraseñas no coinciden
                    throw new ArgumentException("La nueva contraseña y la confirmación no coinciden.");
                }
            }

            // 4. Guardar cambios de forma atómica
            _context.Accounts.Update(account);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
