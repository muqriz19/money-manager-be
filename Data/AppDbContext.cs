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
        //     builder.Entity<LogDb>()
        // .ToTable("Logs");

            builder.Entity<Log>().Property(log => log.Category).HasColumnType("json");

            // builder.Entity<Record>()
            // .HasMany(e => e.Logs)
            // .WithOne(e => e.Record)
            // .HasForeignKey(e => e.RecordId);

            // builder.Entity<Log>()
            // .HasOne(e => e.Record)
            // .WithMany(e => e.Logs)
            // .HasForeignKey(e => e.RecordId);

            base.OnModelCreating(builder);
        }
    }
}