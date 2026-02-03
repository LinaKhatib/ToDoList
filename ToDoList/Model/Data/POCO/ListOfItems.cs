using ToDoList.ViewModel;

namespace ToDoList.Model.Data.POCO
{
    class ListOfItems : INumberPossition
    {
        public Guid Id { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int Number { get; set; } = 0;
        public ICollection<Element> Elements { get; set; } = new List<Element>();
    }
}
