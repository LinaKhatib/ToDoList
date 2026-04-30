using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;
using ToDoList.Services.NumberPosition;

namespace ToDoList.ViewModel.WindowViewModels
{
    internal abstract class AbstractViewModel<T> : BaseViewModel, IDisposable where T : class, IIdentifiable, INumberPossition, new()
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
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ExitCommand { get; }

        protected AbstractViewModel(IRepository<T> repository, INavigationService navigationService)
        {
            _repository = repository;
            _navigationService = navigationService;

            AddCommand = new RelayCommand(async (obj) => await AddItemAsync());
            RemoveCommand = new RelayCommand(async (obj) => await OnRemoveCommandExecuted(obj), CanRemove);
            UpdateCommand = new RelayCommand(async (obj) => await UpdateItemAsync(SelectedItem), CanUpdate);
            ExitCommand = new RelayCommand(_ => ExitApplication());

            Items.CollectionChanged += ListsOfItems_CollectionChanged;

            _ = LoadDataAsync();
        }

        protected virtual bool CanRemove(object obj) => obj != null || SelectedItem != null;
        protected virtual bool CanUpdate(object obj) => SelectedItem != null;

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public virtual void Dispose()
        {
            Items.CollectionChanged -= ListsOfItems_CollectionChanged;
            
            if (_selectedItem is INotifyPropertyChanged item)
            {
                item.PropertyChanged -= OnSelectedItemPropertyChanged;
            }
        }

        protected virtual async Task LoadDataAsync()
        {
            var data = (await _repository.GetAllAsync()).OrderBy(x => x.Id).ToList();
            UpdateCollection(data);
            CollectionService.ReorderNumbers<T>(Items);
        }

        protected virtual async Task AddItemAsync()
        {
            var newItem = new T();

            await _repository.AddAsync(newItem);

            Items.Add(newItem);
        }

        protected virtual async Task RemoveItemAsync(T item)
        {
            if (item != null)
            {
                await _repository.DeleteAsync(item);

                Items.Remove(item);
            }
        }
        
        protected virtual async Task UpdateItemAsync(T item)
        {
            await _dbSemaphore.WaitAsync();
            try
            {
                await _repository.UpdateAsync(item);
            }
            finally { _dbSemaphore.Release(); }
        }

        protected virtual async Task OnRemoveCommandExecuted(object obj)
        {
            var itemToRemove = obj as T ?? SelectedItem;

            if (itemToRemove != null)
            {
                await RemoveItemAsync(itemToRemove);
            }
        }

        protected void UpdateCollection(IEnumerable<T> newData)
        {
            Items.Clear();
            foreach (var item in newData)
            {
                Items.Add(item);
            }
        }

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

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is INotifyPropertyChanged npc)
                        npc.PropertyChanged += OnSelectedItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged npc)
                        npc.PropertyChanged -= OnSelectedItemPropertyChanged;
                }
            }
        } 

        protected virtual async void OnSelectedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is T changedItem)
            {
                await UpdateItemAsync(changedItem);
            }
            
        }

    }
}
