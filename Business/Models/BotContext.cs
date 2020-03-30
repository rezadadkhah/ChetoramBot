using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
       public class BotDbContext : DbContext
    {
        const string connectionString = "Server=DAD-R-57\\REZA;Database=BotDB;Trusted_Connection=True;";

        public BotDbContext() : base() { }

        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Survey> Survey { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
