using System.Windows;
using System.Windows.Controls;
using ToDoList.ViewModel;

namespace ToDoList.View
{
    internal class ChoosingTheElementStyle : StyleSelector  //  выбор стиля для нового элемента
                                                            //  или динамическая насторйка стилей элементов в зависимости
                                                            //  от их порядкового номера при удалении части элементов
    {
        public Style OddButtonStyle { get; set; }
        public Style EvenButtonStyle { get; set; }
        public Style SpecialButtonStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {

            var entity  = item as INumberPossition;

            if (entity != null)
            {
                if (entity.Number == 0) return EvenButtonStyle;
                else return entity.Number % 2 == 0 ? EvenButtonStyle : OddButtonStyle;
            }
            return base.SelectStyle(item, container);
        }
    }
}
