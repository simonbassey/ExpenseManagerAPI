using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseMgr.Services.Abstractions;
using ExpenseMrg.Domain;
using ExpenseMgr.Data.Repositories;
using ExpenseMgr.Domain.Models;
using ExpenseMgr.Services.Helpers;
using System.Text.RegularExpressions;

namespace ExpenseMgr.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly ICurrencyConverter currencyConverter;
        public ExpenseService(IExpenseRepository expenseRepository_, ICurrencyConverter converter)
        {
            expenseRepository = expenseRepository_;
            currencyConverter = converter;
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

        public async Task<Expense> SaveExpense(ExpenseViewModel expenseVm)
        {
            try
            {
                if (expenseVm == null)
                    throw new ArgumentException($"expense cannot be null");
                var expenseRecordExist = await expenseRepository.ExpenseExists(
                                                expenseVm.Title, expenseVm.ExpenseDate.HasValue ?
                                                expenseVm.ExpenseDate.Value : DateTime.Now);
                if (expenseRecordExist)
                    return null;
                var expense = BuildExpenseRecordFromModel(expenseVm);
                expense.Amount = !expenseVm.Amount.Trim().EndsWith("EUR", StringComparison.InvariantCultureIgnoreCase) ?
                    double.Parse(expenseVm.Amount) : await SetAmount(expenseVm.Amount);

                return await expenseRepository.SaveExpense(expense);

            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<double> SetAmount(string amountStr)
        {
            var digits = Regex.Match(amountStr, @"^\d+").Value;
            double amount = double.Parse(digits);
            var value = await currencyConverter.Convert(amount, "EUR", "GBP");
            return value > -1 ? value : amount;
        }

        private Expense BuildExpenseRecordFromModel(ExpenseViewModel expense)
        {
            var expenseRecord = new Expense
            {
                UserId = Guid.Parse(expense.UserId),
                ExpenseDate = expense.ExpenseDate.HasValue ? expense.ExpenseDate : DateTime.Now,
                Description = expense.Description,
                Title = expense.Title
            };
            return expenseRecord;
        }
    }
}
