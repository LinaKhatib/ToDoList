using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;
using ToDoList.Services.NumberPosition;
using ToDoList.Services.ParentNameUpdate;


namespace ToDoList.ViewModel.WindowViewModels
{
    internal class ElementsWindowViewModel : AbstractViewModel<Element>
    {

        private readonly Guid _parentId;
        public ListOfItems ParentList { get; set; }

        
        public ElementsWindowViewModel(IRepository<Element> repoElement, INavigationService navigationService, ListOfItems parentList) 
            : base (repoElement, navigationService)
        {
            _parentId = parentList.Id;
            ParentList = parentList;

            GoBackCommand = new RelayCommand(_ => GoBackToMainOpenWindow());

            ParentList.PropertyChanged += OnParentPropertyChanged;
        }

        protected override async Task LoadDataAsync()
        {
            var allData = await _repository.GetAllAsync();

            var filteredData = allData
                .Where(x => x.CollectionOfListsId == _parentId)
                .OrderBy(x => x.Id)
                .ToList();

            UpdateCollection(filteredData);

            CollectionService.ReorderNumbers<Element>(Items);

            SelectedItem = Items.FirstOrDefault();
        }


        protected override async Task AddItemAsync()
        {
            await _dbSemaphore.WaitAsync();
            try
            {
                var newItem = new Element();
                newItem.Id = Guid.NewGuid();
                newItem.CollectionOfListsId = _parentId;

                await _repository.AddAsync(newItem);

                Items.Add(newItem);
                SelectedItem = newItem;

                if (Items.Count == 1)
                {
                    ParentName.UpdateParentNameLogic(Items, ParentList);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ОШИБКА: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($"ДЕТАЛИ: {ex.InnerException.Message}");
            }
            finally
            {
                _dbSemaphore.Release();
            }
        }
        public ICommand GoBackCommand { get; }

        private void GoBackToMainOpenWindow()
        {
            _navigationService.GoBackToMain();
        }

        internal void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ParentList.CollectionName2))
            {
                ParentName.UpdateParentNameLogic(Items, ParentList);
            }
        }
        protected override void OnSelectedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnSelectedItemPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Element.ThingName))
            {
                _ = UpdateItemAsync();
                ParentName.UpdateParentNameLogic(Items, ParentList);
            }
        }
    }
}
