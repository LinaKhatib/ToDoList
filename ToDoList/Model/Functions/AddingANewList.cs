using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ToDoList.Model.Data;

namespace ToDoList.Model.Functions
{
    class AddingANewList
    {
        public void AddItemToWindow(MainWindow window)
        {
            var newItem = new ListBoxItem();
            var newButton = new Button();

            newItem.Style = (Style)window.FindResource("ListBoxItemForListItems");

            if (window.ListItemTopics.Items.Count % 2 == 0)
                newButton.Style = (Style)window.FindResource("ButtonElementLight");
            else
                newButton.Style = (Style)window.FindResource("ButtonElementDark");

            newItem.Content = newButton;

            window.ListItemTopics.Items.Insert(window.ListItemTopics.Items.Count - 1, newItem);

            CollectionOfLists list1 = new CollectionOfLists { CollectionName = "collection1", NumberPossition = 1 };
        }

    }
}
