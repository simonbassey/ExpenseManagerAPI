using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseMgr.Services.Abstractions;
using ExpenseMrg.Domain;
using ExpenseMgr.Data.Repositories;

namespace ExpenseMgr.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository expenseRepository;
        public ExpenseService(IExpenseRepository expenseRepository_)
        {
            expenseRepository = expenseRepository_;
        }

        public async Task<Expense> GetExpense(int id)
        {
            try
            {
                return await expenseRepository.GetExpense(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Expense>> GetExpenses()
        {
            try
            {
                return await expenseRepository.GetExpenses();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Expense>> GetExpenses(string userId)
        {
            try
            {
                var userExpenses = await expenseRepository.FilterExpensis(e => e.UserId.ToString().Equals(userId));
                return userExpenses;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Expense>> GetExpenses(Func<Expense, bool> searchCondition)
        {
            try
            {
                var filteredExpenses = await expenseRepository.FilterExpensis(searchCondition);
                return filteredExpenses;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Expense> SaveExpense(Expense expense)
        {
            try
            {
                if (expense == null)
                    throw new ArgumentException($"{nameof(expense)} cannot be null");
                var expenseAlreadyRecorded = await expenseRepository.ExpenseExists(expense.Title, expense.ExpenseDate.HasValue ?
                                                                             expense.ExpenseDate.Value : DateTime.Now);
                if (expenseAlreadyRecorded)
                    return null;
                return await expenseRepository.SaveExpense(expense);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
