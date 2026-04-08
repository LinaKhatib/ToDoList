using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;

namespace ToDoList.ViewModel.WindowViewModels
{
    class MainWindowViewModel : AbstractViewModel<ListOfItems>
    {
        public MainWindowViewModel(IRepository<ListOfItems> repoListOfItems, INavigationService navigationService) 
            : base(repoListOfItems, navigationService)
        {
            OpenElementCommand = new RelayCommand<ListOfItems>(OpenElement);
            ExitCommand = new RelayCommand(_ => ExitApplication());
        }

        protected override async Task AddItemAsync()
        {
            var newItem = new ListOfItems();
            newItem.CollectionName2 = newItem.Date.ToString();

            await _repository.AddAsync(newItem);

            Items.Add(newItem);
        }

        public ICommand OpenElementCommand { get; }
        public ICommand ExitCommand { get; }

        public void OpenElement(ListOfItems SelectedItem)
        {
            if (SelectedItem != null)
            {
                _navigationService.OpenWindow(SelectedItem);
            }
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
        public void ClearSelection()
        {
            SelectedItem = null;
        }
    }
} 
