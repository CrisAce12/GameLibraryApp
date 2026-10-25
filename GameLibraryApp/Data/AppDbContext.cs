using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibraryApp.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Models.Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=gamelibrary.db");
        }

    }

}
