using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IDeleteUserRepository
    {
        Task<bool> DeleteUser(int idAccount);
    }
}
