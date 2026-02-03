using System.Windows;
using System.Windows.Controls;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Model.Functions
{
    class AddingANewList // добавление нового элемента в список (не по паттерну MVVM)
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

            Element list1 = new Element { CollectionName = "collection1", Number = 1 };
        }

    }
}
