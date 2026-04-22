using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using ToDoList.Model.Data.DatabaseProvider.Configurations;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Model.Data.DatabaseProvider
{
    class ApplicationContex : DbContext  
    {
        //private readonly string _fileWithDBConnection = "Model/Data/DatabaseProvider/appsettings.json";
        public DbSet<Element> Elements { get; set; } = null!;
        public DbSet<ListOfItems> ListsOfItems { get; set; } = null!;

        public ApplicationContex(DbContextOptions<ApplicationContex> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ListOfItemsConfiguration());
            modelBuilder.ApplyConfiguration(new ElementConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
