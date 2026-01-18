using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Model.Data
{
    class ListOfElements
    {
        public Guid Id { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int NumberPossition { get; set; }
        public string? ThingName { get; set; } = string.Empty;
        public bool Condition { get; set; } = false;
        public Guid CollectionOfListsId { get; set; }
        public CollectionOfLists? Collection { get; set; }
    }
}
