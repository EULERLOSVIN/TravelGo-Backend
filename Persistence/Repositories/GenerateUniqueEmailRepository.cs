using Application.Interfaces.ManagementUser;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Globalization;
using System.Text;

namespace Persistence.Repositories
{
    public class GenerateUniqueEmailRepository: IGenerateUniqueEmailRepository
    {
        private readonly ApplicationDbContext _context;
        public GenerateUniqueEmailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> GenerateUniqueEmail(string firstName, string lastName, int? excludeAccountId = null)
        {
            // Limpiamos espacios y quitamos tildes antes de procesar
            string cleanFirstName = RemoveAccents(firstName.Trim().Split(' ')[0].ToLower());
            string cleanLastName = RemoveAccents(lastName.Trim().Split(' ')[0].ToLower());

            string baseEmail = $"{cleanFirstName}.{cleanLastName}";
            string domain = "@travelgo.com";
            string finalEmail = baseEmail + domain;

            int counter = 1;

            while (await _context.Accounts.AnyAsync(a => a.Email == finalEmail && (!excludeAccountId.HasValue || a.IdAccount != excludeAccountId.Value)))
            {
                finalEmail = $"{baseEmail}{counter}{domain}";
                counter++;
            }

            return finalEmail;
        }

        private string RemoveAccents(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
