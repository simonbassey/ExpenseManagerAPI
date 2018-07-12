using System;
using System.Threading.Tasks;
using ExpenseMgr.Domain;
using System.Collections;
using System.Collections.Generic;

namespace ExpenseMgr.Services.Abstractions
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUser(string userId);
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<User>> GetUsers(Func<User, bool> criteria);
    }
}
