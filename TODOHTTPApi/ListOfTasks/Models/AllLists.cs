using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListOfTasks.Models
{
    public static class AllLists
    {
        //Зберігає всі користувацькі списки і задачі
        public static List<ToDoList> _AllLists = new List<ToDoList>();
        //Назва робочого списку
        public static string _currentList;
        //Розумні списки
        public static ToDoList all_tasks = new ToDoList();
        public static ToDoList planned_tasks = new ToDoList();
        public static ToDoList important_tasks = new ToDoList();
        public static ToDoList today_tasks = new ToDoList();
        //Змінна для отримання всіх задач один раз
        public static bool first = true;
    }
}
