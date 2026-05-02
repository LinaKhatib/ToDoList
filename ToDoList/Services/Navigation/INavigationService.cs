namespace ToDoList.Services.Navigation
{
    internal interface INavigationService
    {
        void OpenWindow<T>(T parameter);
        void GoBackToMain();
    }
}
