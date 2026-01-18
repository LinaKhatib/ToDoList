using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ToDoList.Model.Data
{
    class CollectionOfLists
    {
        public Guid Id { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public DateOnly Date { get; set; } 
        public int NumberPossition { get; set; } = 0;
        public ICollection<ListOfElements> Lists { get; set; } = [];
    }
}
