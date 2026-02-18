using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.ManagementUser;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class EditUserRepository: IEditUserRepository
    {
        public readonly ApplicationDbContext _context;
        private readonly IGenerateUniqueEmailRepository _generateUniqueEmailRepository;

        public EditUserRepository(ApplicationDbContext context , IGenerateUniqueEmailRepository generateUniqueEmailRepository)
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

            // 1. Optimización lógica: Solo generar email si realmente hay un cambio de nombre/apellido
            if (person.FirstName != newData.FirstName || person.LastName != newData.LastName)
            {
                account.Email = await _generateUniqueEmailRepository.GenerateUniqueEmail(newData.FirstName, newData.LastName);
            }

            // Actualización de campos
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

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
