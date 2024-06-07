using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Models;
using Newtonsoft.Json;

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
            builder.Entity<Log>().Property(log => log.Category).HasColumnType("json");

            base.OnModelCreating(builder);
        }
    }
}