using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model.Data.POCO;
using ToDoList.Model.Data.Repositories;

namespace ToDoList.Services.Navigation
{
    internal interface INavigationService
    {
        void OpenWindow<T>(T parameter);
        void GoBackToMain();
    }
}
