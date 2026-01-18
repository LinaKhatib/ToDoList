using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Channels;
using System.Windows.Input;
using ToDoList.Model.Data;

namespace ToDoList.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        //public ICommand SaveCommand { get; }

        //private BaseViewModel baseViewModel;

        public ObservableCollection<CollectionOfLists> CollectionsOfLists { get; set; }

        public MainWindowViewModel()
        {
            // Заполняем тестовыми данными
            CollectionsOfLists = new ObservableCollection<CollectionOfLists>
            {
                new CollectionOfLists { CollectionName = "collection1", NumberPossition = 1 },
                new CollectionOfLists { CollectionName = "collection2", NumberPossition = 2 }
            };
        }
    }
} 
