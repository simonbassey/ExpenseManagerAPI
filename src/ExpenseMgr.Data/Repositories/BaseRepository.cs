using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseMgr.Data.Abstractions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace ExpenseMgr.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        public virtual async Task<T> AddAsync(T tEntity)
        {
            using (var db = new ExpenseMgrContext())
            {
                await db.Set<T>().AddAsync(tEntity);
                return (await db.SaveChangesAsync()) == 1 ? tEntity : null;
            }
        }

        public virtual async Task<IEnumerable<T>> FilterAsync(Func<T, bool> criteria)
        {
            using (var db = new ExpenseMgrContext())
            {
                return await Task.FromResult(db.Set<T>().Where(criteria).ToList());
            };

        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var db = new ExpenseMgrContext())
            {
                return await db.Set<T>().ToListAsync();
            }
        }

        public virtual async Task<T> GetAsync(params object[] key)
        {
            using (var db = new ExpenseMgrContext())
            {
                return await db.Set<T>().FindAsync(key);
            }
        }
    }
}
