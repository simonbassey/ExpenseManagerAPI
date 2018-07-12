using System;
using System.Linq;
using System.Threading.Tasks;
using ExpenseMgr.Domain;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System.Collections;
using System.Collections.Generic;

namespace ExpenseMgr.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> GetUser(string key);
        Task<IEnumerable<User>> Getusers();
        Task<IEnumerable<User>> Search(Func<User, bool> searchQuery);
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository()
        {
        }

        public async Task<User> CreateUser(User user)
        {
            var userExists = await UserExist(user.Email);
            if (userExists)
                return null;
            return await AddAsync(user);
        }

        public async Task<User> GetUser(string key)
        {
            return await GetAsync(key);
        }

        public async Task<IEnumerable<User>> Getusers()
        {
            return await GetAllAsync();
        }

        public async Task<IEnumerable<User>> Search(Func<User, bool> searchQuery)
        {
            return await FilterAsync(searchQuery);
        }

        private async Task<bool> UserExist(string userId)
        {
            return (await FilterAsync(user => user.Email.Equals(userId, StringComparison.OrdinalIgnoreCase) ||
                                      user.UserName.Equals(userId, StringComparison.OrdinalIgnoreCase))).FirstOrDefault() != null;

        }
    }
}
