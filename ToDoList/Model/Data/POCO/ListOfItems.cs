using ToDoList.Services.NumberPosition;
using ToDoList.ViewModel;

namespace ToDoList.Model.Data.POCO
{
    class ListOfItems : BaseViewModel, INumberPossition, IIdentifiable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
         
        private string _collectionName = "Collection";
        public string CollectionName
        {
            get => _collectionName;
            set
            {
                _collectionName = value;
                OnPropertyChanged(nameof(CollectionName));
            }
        }

        private string _collectionName2;
        public string CollectionName2
        {
            get => _collectionName2;
            set
            {
                _collectionName2 = value;
                OnPropertyChanged(nameof(CollectionName2));
            }
        }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int Number { get; set; } = 0;
        public ICollection<Element> Elements { get; set; } = new List<Element>();
    }
}
