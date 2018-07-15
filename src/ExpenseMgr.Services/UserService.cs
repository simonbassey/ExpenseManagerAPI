using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseMgr.Data.Repositories;
using ExpenseMgr.Domain;
using ExpenseMgr.Services.Abstractions;
namespace ExpenseMgr.Services
{
    /// <summary>
    /// User service. Encapsulates business logic for managing user service
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository_)
        {
            userRepository = userRepository_;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                return await userRepository.CreateUser(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = (await userRepository.Search(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                           .FirstOrDefault();
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetUser(string userId)
        {
            try
            {
                return await userRepository.GetUser(userId);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            try
            {
                return await userRepository.Getusers();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetUsers(Func<User, bool> criteria)
        {
            try
            {
                return await userRepository.Search(criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
