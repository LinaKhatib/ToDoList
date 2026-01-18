using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Model.Data
{
    class ApplicationContex : DbContext
    {
        public DbSet<CollectionOfLists> ListBoard { get; set; } = null!;
        public DbSet<ListOfElements> ListsElements { get; set; } = null!;

        public ApplicationContex()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=UsersDB;Username=postgres;Password=Lin789");
        }
    }
}
