using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Models;

namespace moneyManagerBE.Data
{
    public class AppDbContext : DbContext
    {
        // all tables
        public DbSet<Account> Accounts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}