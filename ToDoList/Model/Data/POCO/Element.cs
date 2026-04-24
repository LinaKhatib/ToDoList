using ToDoList.Services.NumberPosition;
using ToDoList.ViewModel;

namespace ToDoList.Model.Data.POCO
{
    class Element : BaseViewModel, INumberPossition, IIdentifiable
    {
        public int Id { get; set; }
        public int Number { get; set; }
        private string _thingName = string.Empty;
        public string ThingName
        {
            get => _thingName;
            set
            {
                _thingName = value;
                OnPropertyChanged(nameof(ThingName));
            }
        }
        public bool IsDone { get; set; } = false;
        public ListOfItems CollectionOfLists { get; set; } = null!;
        public int CollectionOfListsId { get; set; }
    }
}
