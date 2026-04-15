using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Services.ParentNameUpdate
{
    internal static class ParentName
    {
        internal static void UpdateParentNameLogic(ObservableCollection<Element> Items, ListOfItems ParentList)
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
    }
}
