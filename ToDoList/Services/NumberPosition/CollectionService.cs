namespace ToDoList.Services.NumberPosition
{
    internal static class CollectionService // ститический класс (потому что не хнанит никакого состояния,
                                            // выполняет всего 1 функцию и освобождает от создания объектов)
                                            // реализующий статический метод с обобщением,который осуществляет
                                            // последовательную нумирацию элементов какого-либо списка
                                            // (какого-либо типа)
    {
        public static void ReorderNumbers<T>(IEnumerable<T> collection) where T : INumberPossition
        {
            int index = 0;
            foreach (var item in collection)
            {
                item.Number = index++;
            }
        }
    }
}
