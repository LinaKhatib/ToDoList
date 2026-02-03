using ToDoList.ViewModel;

namespace ToDoList.Model.Data.POCO
{
    class Element : INumberPossition
    {
        public Guid Id { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public int Number { get; set; }
        public string? ThingName { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
        public ListOfItems CollectionOfLists { get; set; } = null!;
        public Guid CollectionOfListsId { get; set; }
    }
}
