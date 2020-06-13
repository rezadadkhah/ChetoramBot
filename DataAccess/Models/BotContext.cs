using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class BotDbContext : DbContext
    {
        const string connectionString = "Server=DAD-R-57\\REZA;Database=BotDB;Trusted_Connection=True;";

        public BotDbContext() : base() { }


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<UserSurvey> UserSurveys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
