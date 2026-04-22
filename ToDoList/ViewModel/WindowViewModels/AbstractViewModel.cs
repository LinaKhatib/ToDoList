using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;
using ToDoList.Services.NumberPosition;

namespace ToDoList.ViewModel.WindowViewModels
{
    internal abstract class AbstractViewModel<T> : BaseViewModel where T : class, IIdentifiable, INumberPossition, new()
    {
        protected readonly IRepository<T> _repository;
        protected readonly INavigationService _navigationService;

        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();

        protected readonly SemaphoreSlim _dbSemaphore = new SemaphoreSlim(1, 1);
        protected bool _isSorting;

        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem is INotifyPropertyChanged oldItem)
                {
                    oldItem.PropertyChanged -= OnSelectedItemPropertyChanged;
                }


                _selectedItem = value;
                if (_selectedItem is INotifyPropertyChanged newItem)
                {
                    newItem.PropertyChanged += OnSelectedItemPropertyChanged;
                }

                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand UpdateCommand { get; }

        protected AbstractViewModel(IRepository<T> repository, INavigationService navigationService)
        {
            _repository = repository;
            _navigationService = navigationService;

            AddCommand = new RelayCommand(async (obj) => await AddItemAsync());
            RemoveCommand = new RelayCommand(async (obj) => await RemoveItemAsync(), CanRemove);
            UpdateCommand = new RelayCommand(async (obj) => await UpdateItemAsync(), CanUpdate);

            Items.CollectionChanged += ListsOfItems_CollectionChanged;

            _ = LoadDataAsync();
        }

        protected virtual async Task LoadDataAsync()
        {
            var data = (await _repository.GetAllAsync()).OrderBy(x => x.Number).ToList();
            UpdateCollection(data);
            CollectionService.ReorderNumbers<T>(Items);
        }

        protected virtual async Task AddItemAsync()
        {
            var newItem = new T();

            await _repository.AddAsync(newItem);

            Items.Add(newItem);
        }

        protected virtual async Task RemoveItemAsync()
        {
            if (SelectedItem != null)
            {
                await _repository.DeleteAsync(SelectedItem);

                Items.Remove(SelectedItem);
            }
        }
        
        protected virtual async Task UpdateItemAsync()
        {
            if (SelectedItem != null)
            {
                await _dbSemaphore.WaitAsync();
                try
                {
                    await _repository.UpdateAsync(SelectedItem);
                }
                finally { _dbSemaphore.Release(); }
            }
        }


        protected void UpdateCollection(IEnumerable<T> newData)
        {
            var itemsToRemove = Items.Where(existing => !newData.Any(n => n.Id == existing.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                Items.Remove(item);
            }

            foreach (var newItem in newData)
            {
                if (Items.All(i => i.Id != newItem.Id))
                {
                    Items.Add(newItem);
                }
            }
        }

        protected virtual bool CanRemove(object obj) => SelectedItem != null;
        protected virtual bool CanUpdate(object obj) => SelectedItem != null;


        protected virtual void ListsOfItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_isSorting) return;

            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove)
            {
                _isSorting = true;
                try
                {
                    CollectionService.ReorderNumbers<T>(Items);
                    OnPropertyChanged(nameof(Items));
                }
                finally
                {
                    _isSorting = false;
                }
            }
        } 

        protected virtual async void OnSelectedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await UpdateItemAsync();
        }

    }
}
