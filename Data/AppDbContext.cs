using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Models;

namespace moneyManagerBE.Data
{
    public class AppDbContext : DbContext
    {
        // all tables - define them here
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}