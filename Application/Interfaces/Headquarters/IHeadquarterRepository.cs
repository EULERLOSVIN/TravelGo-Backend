using Application.DTOs.Headquarters;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Headquarters
{
    public interface IHeadquarterRepository
    {
        //Task<> significa que es asíncrono (async)
        Task<List<HeadquarterDto>> GetAllAsync();
        Task<HeadquarterDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateHeadquarterDto headquarterDto);
        Task<bool> UpdateAsync(int id, UpdateHeadquarterDto headquarterDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
