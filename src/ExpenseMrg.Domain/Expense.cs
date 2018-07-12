using System;
using System.ComponentModel.DataAnnotations;
using ExpenseMgr.Domain;

namespace ExpenseMrg.Domain
{
    public class Expense
    {


        [Key]
        public int ExpenseId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public virtual User User { get; set; }

    }
}
