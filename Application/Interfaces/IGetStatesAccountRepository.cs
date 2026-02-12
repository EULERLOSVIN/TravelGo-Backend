using Application.DTOs;

namespace Application.Interfaces
{
    public interface IGetStatesAccountRepository
    {
        Task<List<StateOfAccountDto>> GetStatesAccount();
    }
}
