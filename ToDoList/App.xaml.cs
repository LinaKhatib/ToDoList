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
            base.OnStartup(e);

            var commonContext = new ApplicationContex();

            var repository = new Repository<ListOfItems>(commonContext);
            var repositoryElement = new Repository<Element>(commonContext);

            var navigationService = new ItemNavigationService(repositoryElement, repository);
            var viewModel = new MainWindowViewModel(repository, navigationService);

            var mainWindow = new MainWindow { DataContext = viewModel };
            mainWindow.Show();
        }
    }

}
