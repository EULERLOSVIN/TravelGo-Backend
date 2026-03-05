using Application.DTOs.Settings;
using Application.Interfaces.Settings;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Settings
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Application.Interfaces.ManagementUser.IGenerateUniqueEmailRepository _generateUniqueEmailRepository;

        public ProfileRepository(ApplicationDbContext context, Application.Interfaces.ManagementUser.IGenerateUniqueEmailRepository generateUniqueEmailRepository)
        {
            _context = context;
            _generateUniqueEmailRepository = generateUniqueEmailRepository;
        }

        public async Task<ProfileDto?> GetProfileAsync(int idAccount)
        {
            return await _context.Accounts
                .Where(a => a.IdAccount == idAccount)
                .Select(a => new ProfileDto
                {
                    IdAccount = a.IdAccount,
                    FirstName = a.IdPersonNavigation != null ? a.IdPersonNavigation.FirstName : string.Empty,
                    LastName = a.IdPersonNavigation != null ? a.IdPersonNavigation.LastName : string.Empty,
                    DocumentNumber = a.IdPersonNavigation != null ? a.IdPersonNavigation.NumberIdentityDocument : string.Empty,
                    DocumentType = (a.IdPersonNavigation != null && a.IdPersonNavigation.IdTypeDocumentNavigation != null) ? a.IdPersonNavigation.IdTypeDocumentNavigation.Name : string.Empty,
                    PhoneNumber = (a.IdPersonNavigation != null && a.IdPersonNavigation.PhoneNumber != null) ? a.IdPersonNavigation.PhoneNumber : string.Empty,
                    Email = a.Email != null ? a.Email : string.Empty,
                    Role = a.IdRoleNavigation != null ? a.IdRoleNavigation.Name : string.Empty
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProfileAsync(int idAccount, ProfileDto profileDto)
        {
            var account = await _context.Accounts
                .Include(a => a.IdPersonNavigation)
                .FirstOrDefaultAsync(a => a.IdAccount == idAccount);

            if (account == null) return false;

            // Actualizamos Person
            if (account.IdPersonNavigation != null)
            {
                if (account.IdPersonNavigation.FirstName != profileDto.FirstName || account.IdPersonNavigation.LastName != profileDto.LastName)
                {
                    account.Email = await _generateUniqueEmailRepository.GenerateUniqueEmail(profileDto.FirstName, profileDto.LastName, idAccount);
                }

                account.IdPersonNavigation.FirstName = profileDto.FirstName;
                account.IdPersonNavigation.LastName = profileDto.LastName;
                account.IdPersonNavigation.PhoneNumber = profileDto.PhoneNumber;
                account.IdPersonNavigation.NumberIdentityDocument = profileDto.DocumentNumber;
            }

            // Si hay cambio de contraseña, la encriptamos (usando BCrypt temporalmente si es la librería del repo)
            if (!string.IsNullOrEmpty(profileDto.NewPassword))
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(profileDto.NewPassword);
            }

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
