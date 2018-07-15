using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseMgr.Domain.Models
{
    public class ExpenseViewModel
    {
        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Expense amount is required")]
        public string Amount { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ExpenseDate { get; set; } = DateTime.Now;
    }
}
