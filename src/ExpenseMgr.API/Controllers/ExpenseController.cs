using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using ExpenseMgr.Domain.Models;
using ExpenseMgr.Services.Abstractions;
using ExpenseMrg.Domain;
using ExpenseMgr.Domain;
using System.Net;
using System.Text.RegularExpressions;

namespace ExpenseMgr.API.Controllers
{
    [Route("api/[controller]")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService expenseService;
        private readonly IUserService userService;

        public ExpenseController(IExpenseService expenseService_, IUserService uSerService_)
        {
            userService = uSerService_;
            expenseService = expenseService_;
        }

        /// <summary>
        /// Create new Expense
        /// </summary>
        /// <returns>The HttpStatus Code representing request's status.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]ExpenseViewModel expenseVm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = userService.GetUser(expenseVm.UserId);
                if (user == null)
                    return BadRequest("user is not recognized");
                var amountValidationError = ValidateExpenseAmount(expenseVm.Amount);
                if (amountValidationError.Length > 0)
                    return BadRequest(amountValidationError);
                var expenseRecord = await expenseService.SaveExpense(expenseVm);
                if (expenseRecord == null)
                    return NoContent();
                return Created($"expense/{expenseRecord.ExpenseId}", expenseRecord);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Get details of expense with specified expenseId.
        /// </summary>
        /// <returns></returns>
        /// <param name="expenseId">Expense identifier.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int expenseId)
        {
            try
            {
                var expenseRecord = await expenseService.GetExpense(expenseId);
                if (expenseRecord == null)
                    return BadRequest("Cannot find details for requested expense");
                return Ok(await expenseService.GetExpense(expenseId));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Get expense for specified userId
        /// </summary>
        /// <returns><list type="Expense"></list></returns>
        /// <param name="email">Expense identifier.</param>
        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetUserExpsense(string email)
        {
            try
            {
                var user = await userService.GetUserByEmail(email);
                var expenseRecord = await expenseService.GetExpenses(user?.Email);
                return Ok(expenseRecord.ToList());
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        // GET api/expense/
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            try
            {
                return Ok(await expenseService.GetExpenses());
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }



        private string ValidateExpenseAmount(string amount)
        {
            if (string.IsNullOrEmpty(amount))
                return "Invalid expense amount";
            var regexp = Regex.Match(amount, @"(^\d*\.?\d*\s?(EUR)?)$");
            if (!regexp.Success)
                return "Invalid expense amount";
            return string.Empty;
        }
    }
}
