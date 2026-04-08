using System.Windows;
using System.Windows.Controls;
using ToDoList.ViewModel;

namespace ToDoList.View.StateSelection
{
    public class ChoosingTheElementStyle : StyleSelector  //  выбор стиля для нового элемента
                                                            //  или динамическая насторйка стилей элементов в зависимости
                                                            //  от их порядкового номера при удалении части элементов
    {
        public Style OddButtonStyle { get; set; } 
        public Style EvenButtonStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            return IsItAnEvenOrOddItem.IsEven(item) ? EvenButtonStyle : OddButtonStyle;
        }
    }
}
