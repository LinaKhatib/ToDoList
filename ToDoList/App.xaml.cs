using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Windows;
using ToDoList.Model.Data.DatabaseProvider;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;
using ToDoList.View.Windows;
using ToDoList.ViewModel.WindowViewModels;
using System.IO;


namespace ToDoList
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var options = GetOptions();

                using (var context = new ApplicationContex(options))
                {
                    context.Database.EnsureCreated();
                }

                var repository = new Repository<ListOfItems>(options);
                var repositoryElement = new Repository<Element>(options);

                var navigationService = new ItemNavigationService(repositoryElement, repository);
                var viewModel = new MainWindowViewModel(repository, navigationService);

                var mainWindow = new MainWindow { DataContext = viewModel };
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при запуске:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "ToDoList — ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown(1);
            }
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
                var exeDir = Path.GetDirectoryName(Environment.ProcessPath)
                             ?? AppContext.BaseDirectory;

                var configPath = Path.Combine(exeDir, _fileWithDBConnection);

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(exeDir)
                    .AddJsonFile(_fileWithDBConnection, optional: true, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                System.Diagnostics.Debug.WriteLine($"Ищу файл тут: {configPath}");
                System.Diagnostics.Debug.WriteLine($"Файл найден: {File.Exists(configPath)}");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception(
                        $"Строка подключения не найдена! Проверьте, что файл лежит рядом с .exe: {configPath}");
                }

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

    }
}
