using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoList.Model.Data.DatabaseProvider;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.ViewModel;

namespace ToDoList
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var context = new ApplicationContex();
            var repo = new Repository<ListOfItems>(context);
            this.DataContext = new MainWindowViewModel(repo);
        }
        
    }
}