using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseMgr.Data.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T tEntity);
        Task<T> GetAsync(params object[] key);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FilterAsync(Func<T, bool> criteria);
    }
}
