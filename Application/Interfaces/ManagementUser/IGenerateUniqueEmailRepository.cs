

namespace Application.Interfaces.ManagementUser
{
    public interface IGenerateUniqueEmailRepository
    {
        Task<string> GenerateUniqueEmail(string firstName, string lastName, int? excludeAccountId = null);
    }
}
