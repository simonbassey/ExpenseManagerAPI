using System;
using ExpenseMrg.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace ExpenseMgr.Data.Repositories
{
    public interface IExpenseRepository
    {
        Task<Expense> SaveExpense(Expense newExpense);
        Task<Expense> GetExpense(int expenseId);
        Task<IEnumerable<Expense>> GetExpenses();
        Task<IEnumerable<Expense>> FilterExpensis(Func<Expense, bool> searchQuery);
        Task<bool> ExpenseExists(string title, DateTime date);
    }

    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository()
        {
            //we leave here an empty constructor for dependency injection
        }

        public async Task<IEnumerable<Expense>> FilterExpensis(Func<Expense, bool> searchQuery)
        {
            return await FilterAsync(searchQuery);
        }

        public async Task<Expense> GetExpense(int expenseId)
        {
            return await GetAsync(expenseId);
        }

        public async Task<IEnumerable<Expense>> GetExpenses()
        {
            return await GetAllAsync();
        }

        public async Task<Expense> SaveExpense(Expense newExpense)
        {
            try
            {
                var expenseRecordExist = await ExpenseExists(newExpense.Title, newExpense.ExpenseDate.Value);
                if (expenseRecordExist)
                    return null;
                return await AddAsync(newExpense);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ExpenseExists(string title, DateTime date)
        {
            return (await FilterAsync(exp => exp.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                                      (exp.ExpenseDate - date)?.TotalMinutes < 1)).FirstOrDefault() != null;
        }
    }
}
