using System;
using System.Threading.Tasks;
using ExpenseMrg.Domain;
using System.Collections;
using System.Collections.Generic;
using ExpenseMgr.Domain.Models;

namespace ExpenseMgr.Services.Abstractions
{
    public interface IExpenseService
    {
        Task<Expense> SaveExpense(ExpenseViewModel expense);
        Task<Expense> GetExpense(int id);
        Task<IEnumerable<Expense>> GetExpenses();
        Task<IEnumerable<Expense>> GetExpenses(string userId);
        Task<IEnumerable<Expense>> GetExpenses(Func<Expense, bool> searchCondition);
    }
}
