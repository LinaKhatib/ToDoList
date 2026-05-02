using System.Windows;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.View.Windows;
using ToDoList.ViewModel.WindowViewModels;

namespace ToDoList.Services.Navigation
{
    internal class ItemNavigationService : INavigationService
    {
        private readonly IRepository<Element> _repoElement;
        private readonly IRepository<ListOfItems> _repoList;

        public ItemNavigationService(IRepository<Element> repoElement, IRepository<ListOfItems> repoList)
        {
            _repoElement = repoElement;
            _repoList = repoList;
        }

        public void OpenWindow<T>(T parameter)
        {
            var oldWindow = Application.Current.MainWindow;

            Window newWindow = null;

            if (parameter is ListOfItems parentList)
            {
                newWindow = new ElementsWindow();
                newWindow.DataContext = new ElementsWindowViewModel(_repoElement, this, parentList);
            }
            else if (parameter is Element element)
            {
                newWindow = new ElementsWindow();
                var vm = new ElementsWindowViewModel(_repoElement, this, element.CollectionOfLists);
                vm.SelectedItem = element;
                newWindow.DataContext = vm;
            }
            else if (parameter == null) 
            {
                newWindow = new MainWindow();
                newWindow.DataContext = new MainWindowViewModel(_repoList, this);
                newWindow.Show();
            }

            if (newWindow != null)
            {
                Application.Current.MainWindow = newWindow;
                newWindow.Show();

                oldWindow?.Close();
                newWindow.Closed += (s, e) =>
                {
                    if (Application.Current.Windows.Count == 0)
                    {
                        Application.Current.Shutdown();
                    }
                };
            }
        }

        public void GoBackToMain()
        {
            OpenWindow<object>(null);
        }
    }
}

