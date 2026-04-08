using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using ToDoList.Model.Data.DatabaseProvider.Configurations;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Model.Data.DatabaseProvider
{
    class ApplicationContex : DbContext  
    {
        public DbSet<Element> Elements { get; set; } = null!;
        public DbSet<ListOfItems> ListsOfItems { get; set; } = null!;

        public ApplicationContex()
        {

            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ListOfItemsConfiguration());
            modelBuilder.ApplyConfiguration(new ElementConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("Model/Data/DatabaseProvider/appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                System.Diagnostics.Debug.WriteLine($"Ищу файл тут: {AppContext.BaseDirectory}"); // это чтобы не потерять файл со строкой подключения, если вы его создали не в папке DatabaseProvider :)

                bool exists = System.IO.File.Exists(System.IO.Path.Combine(AppContext.BaseDirectory, "Model/Data/DatabaseProvider/appsettings.json"));
                System.Diagnostics.Debug.WriteLine($"Файл найден: {exists}");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Строка подключения не найдена! Проверьте appsettings.json или секреты.");
                }

                optionsBuilder.UseNpgsql(connectionString);
            }

        }
    }
}
