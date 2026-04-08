using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;
using ToDoList.Services.NumberPosition;


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
                newItem.ThingName = "Тестовая задача";

                await _repository.AddAsync(newItem);
                await _repository.SaveAsync();

                Items.Add(newItem);

                SelectedItem = newItem;

                if (Items.Count == 1)
                {
                    UpdateParentNameLogic();
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

        private void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ParentList.CollectionName2))
            {
                UpdateParentNameLogic();
            }
        }

        private void UpdateParentNameLogic()
        {
            var firstItem = Items.FirstOrDefault();

            if (ParentList.CollectionName2 != ParentList.Date.ToString() && ParentList.CollectionName2 != string.Empty)
            {
                ParentList.CollectionName = ParentList.CollectionName2;
            }
            else if (firstItem != null && firstItem.ThingName != string.Empty)
            {
                ParentList.CollectionName = firstItem.ThingName;
            }
            else 
            {
                ParentList.CollectionName = "Collection";
            }
        }
        protected override void OnSelectedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnSelectedItemPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Element.ThingName))
            {
                _ = UpdateItemAsync();
                UpdateParentNameLogic();
            }
        }
    }
}
