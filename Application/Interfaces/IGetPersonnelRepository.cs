using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IGetPersonnelRepository
    {
        Task<List<PersonnelDto>> GetPersonnelByFilters(FilterPersonnelDto filters);
    }
}
