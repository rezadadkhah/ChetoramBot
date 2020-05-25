using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class BotDbContext : DbContext
    {
        const string connectionString = "Server=DESKTOP-VEH7CIN;Database=BotDB;Trusted_Connection=True;";

        public BotDbContext() : base() { }


        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<UserSurvey> UserSurvey { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
