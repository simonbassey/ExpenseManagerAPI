using System;
using Microsoft.EntityFrameworkCore;
using ExpenseMrg.Domain;
using ExpenseMgr.Domain;
namespace ExpenseMgr.Data
{
    public class ExpenseMgrContext : DbContext
    {
        public ExpenseMgrContext() { }
        public ExpenseMgrContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ExpenseMgr.db");
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
