using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using ToDoList.Model.Data.DatabaseProvider;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;
using ToDoList.Services.Navigation;
using ToDoList.ViewModel.WindowViewModels;

namespace ToDoList.View.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var context = new ApplicationContex();
            //var repo = new Repository<ListOfItems>(context);
            //var nav = new ItemNavigationService(mainWindow);
            //this.DataContext = new MainWindowViewModel(repo, nav);
        }
    }
}