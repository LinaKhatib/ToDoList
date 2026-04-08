using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Services.NumberPosition;

namespace ToDoList.View.StateSelection
{
    internal static class IsItAnEvenOrOddItem
    {
        public static bool IsEven(object item)
        {
            if (item is INumberPossition entity)
            {
                return entity.Number % 2 == 0;
            }
            return false;
        }
    }
}
