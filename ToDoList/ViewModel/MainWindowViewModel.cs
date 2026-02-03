using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services;

namespace ToDoList.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        private readonly IRepository<ListOfItems> _repoListOfItems;

        private ListOfItems _selectedItem;
        public ObservableCollection<ListOfItems> CollectionsOfLists { get; set; }

        public ListOfItems SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public MainWindowViewModel(IRepository<ListOfItems> repoListOfItems)
        {
            _repoListOfItems = repoListOfItems;
            CollectionsOfLists = new ObservableCollection<ListOfItems>();

            AddCommand = new RelayCommand(async (obj) => await AddItemAsync());
            RemoveCommand = new RelayCommand(async (obj) => await RemoveItemAsync(), CanRemove);
            CollectionsOfLists.CollectionChanged += ListsOfItems_CollectionChanged;

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var data = await _repoListOfItems.GetAllAsync();
            foreach (var item in data)
            {
                CollectionsOfLists.Add(item);
            }
        }

        private async Task AddItemAsync()
        {
            var newItem = new ListOfItems();

            await _repoListOfItems.AddAsync(newItem);
            await _repoListOfItems.SaveAsync(); 

            CollectionsOfLists.Add(newItem);
        }

        private async Task RemoveItemAsync()
        {
            if (SelectedItem != null)
            {
                _repoListOfItems.Delete(SelectedItem);
                await _repoListOfItems.SaveAsync(); 

                CollectionsOfLists.Remove(SelectedItem);
            }
        }

        private bool CanRemove(object obj) => SelectedItem != null;


        private void ListsOfItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) // вызывает метод ReorderNumbers класса CollectionService,
                                                                                                       // который делает перерасчет порядковых номеров списка CollectionsOfLists,
                                                                                                       // чтобы обновить или задать нужный стиль элементов
        {
            CollectionService.ReorderNumbers<ListOfItems>(CollectionsOfLists);
            OnPropertyChanged(nameof(CollectionsOfLists));
        }
    }
} 
