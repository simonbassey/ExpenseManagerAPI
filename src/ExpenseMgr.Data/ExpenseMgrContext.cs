using System;
using Microsoft.EntityFrameworkCore;
using ExpenseMrg.Domain;
using ExpenseMgr.Domain;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Diagnostics;
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
            optionsBuilder.UseMySql(GetConnectionString());
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<User> Users { get; set; }



        #region privates
        private string GetConnectionString()
        {
            IConfiguration configuration = StaticServiceResolver.Resolve<IConfiguration>();
            var connectionString = string.Empty;
#if DEBUG
            connectionString = configuration.GetConnectionString("trackyaexpenseDbConnection");
#elif !DEBUG
            connectionString =  configuration["trackyaexpenseDbConnection"];
#endif
            return connectionString;
        }

        #endregion
    }
}
