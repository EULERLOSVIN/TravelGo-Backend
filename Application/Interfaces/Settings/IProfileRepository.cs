using Application.DTOs.Settings;

namespace Application.Interfaces.Settings
{
    public interface IProfileRepository
    {
        Task<ProfileDto?> GetProfileAsync(int idAccount);
        Task<bool> UpdateProfileAsync(int idAccount, ProfileDto profileDto);
    }
}
