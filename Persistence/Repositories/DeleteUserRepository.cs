using Domain.Entities;
﻿using Application.Interfaces;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class DeleteUserRepository: IDeleteUserRepository
    {
        private readonly ApplicationDbContext _context;

        public DeleteUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteUser(int idAccount)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.IdAccount == idAccount);
            if (user != null) {
                var person = _context.People.FirstOrDefault(x => x.IdPerson == user.IdPerson);
                _context.Accounts.Remove(user);
                _context.People.Remove(person!);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
