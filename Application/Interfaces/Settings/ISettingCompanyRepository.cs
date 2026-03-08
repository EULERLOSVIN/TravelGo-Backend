using Application.DTOs.Settings;
using System.Threading.Tasks;

namespace Application.Interfaces.Settings
{
    public interface ISettingCompanyRepository
    {
        Task<SettingCompany> GetCompanyAsync();
        Task<bool> UpdateCompanyAsync(SettingCompany dto);
    }
}
