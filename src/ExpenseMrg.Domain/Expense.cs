using System;
using System.ComponentModel.DataAnnotations;
using ExpenseMgr.Domain;

namespace ExpenseMrg.Domain
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public double Description { get; set; }


        public virtual User User { get; set; }

    }
}
