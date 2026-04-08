using System.Windows;
using System.Windows.Controls;
using ToDoList.ViewModel;

namespace ToDoList.View.StateSelection
{
    public class ChoosingTheDataTemplate : DataTemplateSelector  //  выбор стиля для нового элемента
                                                            //  или динамическая насторйка стилей элементов в зависимости
                                                            //  от их порядкового номера при удалении части элементов
    {
        public DataTemplate OddButtonDataTemplate { get; set; } 
        public DataTemplate EvenButtonDataTemplate { get; set; } 

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return IsItAnEvenOrOddItem.IsEven(item) ? EvenButtonDataTemplate : OddButtonDataTemplate;
        }
    }
}
