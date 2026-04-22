using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ToDoList.Model.Data.DatabaseProvider;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;
using ToDoList.View.Windows;
using ToDoList.ViewModel.WindowViewModels;

namespace ToDoList
{
    public partial class App : Application
    {
       

        protected override void OnStartup(StartupEventArgs e)
        {
            var options = GetOptions();

            using (var context = new ApplicationContex(options))
            {
                context.Database.EnsureDeleted(); // Пока в приложении не появится функционал удаления элементов
                context.Database.EnsureCreated();
            }

            var repository = new Repository<ListOfItems>(options);
            var repositoryElement = new Repository<Element>(options);

            var navigationService = new ItemNavigationService(repositoryElement, repository);
            var viewModel = new MainWindowViewModel(repository, navigationService);

            var mainWindow = new MainWindow { DataContext = viewModel };
            mainWindow.Show();
        }

        private DbContextOptions<ApplicationContex> GetOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContex>();
            GetDBConnectionString(optionsBuilder);
            return optionsBuilder.Options;
        }

        private readonly string _fileWithDBConnection = "Model/Data/DatabaseProvider/appsettings.json";
        private void GetDBConnectionString(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(_fileWithDBConnection, optional: true, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                System.Diagnostics.Debug.WriteLine($"Ищу файл тут: {AppContext.BaseDirectory}"); // это чтобы не потерять файл со строкой подключения, если вы его создали не в папке DatabaseProvider :)

                bool exists = System.IO.File.Exists(System.IO.Path.Combine(AppContext.BaseDirectory, _fileWithDBConnection));
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
